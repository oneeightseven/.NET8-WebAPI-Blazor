namespace Mgeek.Services.ShoppingCartAPI.Controllers;

[Route("api/CartApi")]
[ApiController]
public class CartApiController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ResponseDto _response;
    private readonly IPromoService _promoService;
    private readonly ApplicationDbContext _context;
    private readonly IProductService _productService;

    public CartApiController(IProductService productService, IPromoService promoService, IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
        _promoService = promoService;
        _response = new ResponseDto();
        _productService = productService;
    }
    
    [Authorize]
    [HttpGet("GetCart")]
    public async Task<ResponseDto> GetCart()
    {
        try
        {
            var sub = User.Claims.Where(x => x.Type == IdentityClaims.Sub)?.FirstOrDefault()?.Value;
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == sub);

            IEnumerable<CartDetails> cartDetails = await _context.CartDetails.Where(x => x.CartHeaderId == cartHeader!.Id).ToListAsync();
            IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

            foreach (var cartDetail in cartDetails)
            {
                cartDetail.Product = productDtos.FirstOrDefault(x => x.Id == cartDetail.ProductId);
                cartHeader!.CartTotal += cartDetail.Count * cartDetail.Product!.Price;
            }

            CartDto cart = new()
            {
                CartHeader = _mapper.Map<CartHeaderDto>(cartHeader),
                CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(cartDetails),
            };

            if (!string.IsNullOrEmpty(cart.CartHeader.PromoCode))
            {
                var promo = await _promoService.GetPromo(cart.CartHeader.PromoCode);

                if (cart.CartHeader.CartTotal >= promo.MinAmount)
                {
                    cart.CartHeader.CartTotal -= promo.SumDiscount;
                    cart.CartHeader.Discount = promo.SumDiscount;
                }
            }
            _response.Result = cart;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{Exception}", ex.Message);
        }

        return _response;
    }
    
    [Authorize]
    [HttpPost("CartUpsert")]
    public async Task<ResponseDto> CartUpsert([FromBody] CartDto cartDto)
    {
        try
        {
            var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cartDto.CartHeader!.UserId);
            if (cartHeader == null)
            {
                cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                await _context.CartHeaders.AddAsync(cartHeader);
                await _context.SaveChangesAsync();

                cartDto.CartDetails!.First().CartHeaderId = cartHeader.Id;
                await _context.CartDetails.AddAsync(_mapper.Map<CartDetails>(cartDto.CartDetails!.First()));
                await _context.SaveChangesAsync();
            }
            else
            {
                var cartDetails = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == cartDto.CartDetails!.First().ProductId && x.CartHeaderId == cartHeader.Id);
                if (cartDetails == null)
                {
                    cartDto.CartDetails!.First().CartHeaderId = cartHeader.Id;
                    await _context.CartDetails.AddAsync(_mapper.Map<CartDetails>(cartDto.CartDetails!.First()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cartDto.CartDetails!.First().Count += cartDetails.Count;
                    cartDto.CartDetails!.First().CartHeaderId = cartDetails.CartHeaderId;
                    cartDto.CartDetails!.First().Id = cartDetails.Id;
                    _context.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails!.First()));
                    await _context.SaveChangesAsync();
                }
            }

            _response.Result = cartDto;
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message;
            _response.IsSuccess = false;
            Log.Error("{Exception}", ex.Message);
        }

        return _response;
    }
    
    [Authorize]
    [HttpPost("ChangePromo")]
    public async Task<ResponseDto> ChangePromo([FromBody] CartDto cartDto)
    {
        try
        {
            var cart = await _context.CartHeaders.FirstAsync(x => x.UserId == cartDto.CartHeader!.UserId);

            if (cartDto.CartHeader!.PromoCode != string.Empty & cartDto.CartHeader!.PromoCode != null)
            {
                var promo = await _promoService.GetPromo(cartDto.CartHeader.PromoCode);
                if (promo != null && promo.Remainder > 0)
                {
                    cart.PromoCode = cartDto.CartHeader!.PromoCode;
                    _response.IsSuccess = true;
                    _response.Message = "Promo code applied successfully";
                }
                else
                {
                    cart.PromoCode = string.Empty;
                    _response.IsSuccess = false;
                    _response.Message = "Promo code has expired or not found";
                }
            }
            else
            {
                cart.PromoCode = string.Empty;
                _response.IsSuccess = false;
            }

            _context.CartHeaders.Update(cart);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.ToString();
            Log.Error("{Exception}", ex.Message);
        }
        return _response;
    }
    
    [Authorize]
    [HttpPost("RemoveCart/{cartDetailsId}")]
    public async Task<ResponseDto> RemoveCart(int cartDetailsId)
    {
        try
        {
            var cartDetails = await _context.CartDetails.FirstAsync(x => x.Id == cartDetailsId);
            int totalCountOfCartItem = await _context.CartDetails.Where(x => x.CartHeaderId == cartDetails.Id).CountAsync();
            _context.CartDetails.Remove(cartDetails);
            if (totalCountOfCartItem == 1)
            {
                var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.Id == cartDetails.CartHeaderId);
                _context.CartHeaders.Remove(cartHeader!);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message;
            _response.IsSuccess = false;
            Log.Error("{Exception}", ex.Message);
        }

        return _response;
    }

    [Authorize]
    [HttpPost("RemoveCartPermanently")]
    public async Task<ResponseDto> RemoveCartPermanently()
    {
        try
        {
            var sub = User.Claims.Where(x => x.Type == IdentityClaims.Sub)?.FirstOrDefault()?.Value;
            var result = await _context.CartHeaders.Where(x => x.UserId == sub).ExecuteDeleteAsync();
            if (result == 1)
            {
                _response.IsSuccess = true;
                _response.Message = "Cart empty successfully";
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Could not find the cart with the specified user id";
            }
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message;
            _response.IsSuccess = false;
            Log.Error("{Exception}", ex.Message);
        }
        return _response;
    }
}