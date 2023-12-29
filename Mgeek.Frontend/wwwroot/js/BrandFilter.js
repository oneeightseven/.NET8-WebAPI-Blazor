document.querySelector('#brandFilter').addEventListener('change', function(e) {
    let selectedBrand = e.target.value;
    let productCards = document.querySelectorAll('.product-card');
    productCards.forEach(function(card) {
        let brand = card.querySelector('.brand').innerText;
        if (selectedBrand === '' || selectedBrand === brand)
            card.style.display = 'block';
        else
            card.style.display = 'none';
    });
});