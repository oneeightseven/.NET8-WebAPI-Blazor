const productCards = document.querySelectorAll('.product-card');

const cardsArray = Array.from(productCards);

document.querySelector('#sortFilter').addEventListener('change', () => {
    const selectedSorting = document.querySelector('#sortFilter').value;
    switch (selectedSorting){
        case 'priceAscending':
            sortByPriceAscending();
            break;
        case 'priceDescending':
            sortByPriceDescending();
            break;
    }
});

function sortByPriceAscending() {
    cardsArray.sort((a, b) => {
        const priceA = parseFloat(a.querySelector('.price').innerText.replace(/[^0-9.-]+/g, ''));
        const priceB = parseFloat(b.querySelector('.price').innerText.replace(/[^0-9.-]+/g, ''));
        return priceA - priceB;
    });
    updateCardOrder();
}

function sortByPriceDescending() {
    cardsArray.sort((a, b) => {
        const priceA = parseFloat(a.querySelector('.price').innerText.replace(/[^0-9.-]+/g, ''));
        const priceB = parseFloat(b.querySelector('.price').innerText.replace(/[^0-9.-]+/g, ''));
        return priceB - priceA;
    });

    updateCardOrder();
}

function updateCardOrder() {
    const parentContainer = document.querySelector('.card-container');
    parentContainer.innerHTML = '';

    cardsArray.forEach((card) => {
        parentContainer.appendChild(card);
    });
}