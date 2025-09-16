// Debug script for basket functionality
console.log('=== BASKET DEBUG SCRIPT LOADED ===');

// Debug function to test basket functionality
function debugBasket() {
    console.log('=== BASKET DEBUG INFO ===');
    console.log('1. BasketManager exists:', typeof window.basketManager);
    console.log('2. jQuery loaded:', typeof $ !== 'undefined');
    console.log('3. Quick add buttons found:', $('.quick-add-to-basket').length);
    console.log('4. Add to basket buttons found:', $('.add-to-basket').length);
    console.log('5. Mini cart button found:', $('.minicart-btn').length);
    console.log('6. Notification elements found:', $('.notification').length);
    console.log('7. SweetAlert2 loaded:', typeof Swal !== 'undefined');
    
    // Test if buttons have proper data attributes
    $('.quick-add-to-basket').each(function(index) {
        const productId = $(this).data('product-id');
        console.log(`Quick add button ${index + 1}: product-id = ${productId}`);
    });
    
    console.log('=== END BASKET DEBUG ===');
}

// Auto-run debug on page load
$(document).ready(function() {
    setTimeout(debugBasket, 1000); // Wait 1 second for everything to load
});

// Make debug function available globally
window.debugBasket = debugBasket;

// Test add to basket functionality
function testAddToBasket(productId) {
    console.log('Testing add to basket for product:', productId);
    
    if (!productId) {
        console.error('No product ID provided');
        return;
    }
    
    $.ajax({
        url: '/Basket/AddToBasket',
        type: 'POST',
        data: {
            productId: productId,
            quantity: 1
        },
        success: function(response) {
            console.log('Add to basket success:', response);
        },
        error: function(xhr, status, error) {
            console.error('Add to basket error:', xhr.responseText);
        }
    });
}

window.testAddToBasket = testAddToBasket;

// Test get basket count
function testGetBasketCount() {
    console.log('Testing get basket count...');
    
    $.ajax({
        url: '/Basket/GetBasketCount',
        type: 'GET',
        success: function(response) {
            console.log('Get basket count success:', response);
        },
        error: function(xhr, status, error) {
            console.error('Get basket count error:', xhr.responseText);
        }
    });
}

window.testGetBasketCount = testGetBasketCount;

// Add visual indicators for debugging
$(document).ready(function() {
    // Add debug indicators to buttons
    $('.quick-add-to-basket').each(function() {
        const productId = $(this).data('product-id');
        if (!productId) {
            $(this).css({
                'border': '2px solid red',
                'background-color': 'rgba(255, 0, 0, 0.1)'
            });
            console.warn('Quick add button without product-id found:', this);
        } else {
            $(this).css({
                'border': '1px solid green'
            });
        }
    });
    
    // Add click counter for debugging
    let clickCount = 0;
    $(document).on('click', '.quick-add-to-basket', function() {
        clickCount++;
        console.log(`Quick add button clicked ${clickCount} times. Product ID:`, $(this).data('product-id'));
    });
});

console.log('=== BASKET DEBUG SCRIPT READY ===');
console.log('Available debug functions:');
console.log('- debugBasket() - Show debug info');
console.log('- testAddToBasket(productId) - Test add to basket');
console.log('- testGetBasketCount() - Test get basket count');