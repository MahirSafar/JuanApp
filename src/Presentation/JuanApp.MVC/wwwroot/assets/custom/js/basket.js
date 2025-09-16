// Basket functionality
class BasketManager {
    constructor() {
        this.isLoading = false;
        this.init();
        console.log('BasketManager initialized');
    }

    init() {
        this.bindEvents();
        this.updateBasketCount();
        this.checkTransferBasket();
    }

    bindEvents() {
        // Add to basket buttons
        $(document).on('click', '.add-to-basket', (e) => {
            e.preventDefault();
            console.log('Add to basket clicked');
            if (!this.isLoading) {
                this.addToBasket(e.currentTarget);
            }
        });

        // Quick add buttons (without size/color selection)
        $(document).on('click', '.quick-add-to-basket', (e) => {
            e.preventDefault();
            console.log('Quick add to basket clicked');
            if (!this.isLoading) {
                this.quickAddToBasket(e.currentTarget);
            }
        });

        // Mini cart toggle
        $(document).on('click', '.minicart-btn', (e) => {
            e.preventDefault();
            this.toggleMiniCart();
        });
        
        // Close mini cart when clicking overlay
        $(document).on('click', '.minicart-overlay', (e) => {
            this.closeMiniCart();
        });
        
        // Close mini cart when clicking close button
        $(document).on('click', '.minicart-close', (e) => {
            e.preventDefault();
            this.closeMiniCart();
        });

        // Update basket count on page load
        $(window).on('load', () => {
            this.updateBasketCount();
        });
    }

    addToBasket(button) {
        const $button = $(button);
        const productId = $button.data('product-id');
        
        if (!productId) {
            this.showError('Product ID not found');
            return;
        }

        // Get quantity, size, and color from the page
        const quantity = this.getQuantityFromPage();
        const size = this.getSizeFromPage();
        const color = this.getColorFromPage();

        console.log('Adding to basket:', { productId, quantity, size, color });

        this.sendAddToBasketRequest({
            productId: productId,
            quantity: quantity,
            size: size,
            color: color
        }, $button);
    }

    quickAddToBasket(button) {
        const $button = $(button);
        const productId = $button.data('product-id');

        if (!productId) {
            this.showError('Product ID not found');
            return;
        }

        console.log('Quick adding to basket:', productId);

        this.sendAddToBasketRequest({
            productId: productId,
            quantity: 1
        }, $button);
    }

    getQuantityFromPage() {
        const $quantityInput = $('#quantity, .quantity-input, input[name="quantity"]').first();
        const quantity = parseInt($quantityInput.val()) || 1;
        
        // Validate quantity
        const maxQuantity = parseInt($quantityInput.attr('max')) || 999;
        const minQuantity = parseInt($quantityInput.attr('min')) || 1;
        
        if (quantity > maxQuantity) {
            this.showError(`Maximum quantity is ${maxQuantity}`);
            $quantityInput.val(maxQuantity);
            return maxQuantity;
        }
        
        if (quantity < minQuantity) {
            this.showError(`Minimum quantity is ${minQuantity}`);
            $quantityInput.val(minQuantity);
            return minQuantity;
        }
        
        return quantity;
    }

    getSizeFromPage() {
        const $sizeSelect = $('#size, select[name="size"]').first();
        return $sizeSelect.val() || null;
    }

    getColorFromPage() {
        const $colorSelect = $('#color, select[name="color"]').first();
        return $colorSelect.val() || null;
    }

    sendAddToBasketRequest(data, $button) {
        if (this.isLoading) return;
        
        this.isLoading = true;
        const originalText = $button.text();
        const originalIcon = $button.find('i').attr('class');
        
        $button.prop('disabled', true);
        
        // Update button state
        if ($button.find('i').length > 0) {
            $button.find('i').attr('class', 'fa fa-spinner fa-spin');
        } else {
            $button.text('Adding...');
        }

        console.log('Sending add to basket request:', data);

        $.ajax({
            url: '/Basket/AddToBasket',
            type: 'POST',
            data: data,
            success: (response) => {
                console.log('Add to basket response:', response);
                if (response.success) {
                    this.showSuccess(response.message);
                    this.updateBasketCount();
                    this.updateMiniCart();
                    
                    // Update UI elements if needed
                    if (response.basketItem) {
                        this.highlightAddedItem(response.basketItem);
                    }
                } else {
                    this.showError(response.message);
                }
            },
            error: (xhr, status, error) => {
                console.error('Add to basket error:', xhr.responseText);
                this.showError('Error adding product to basket. Please try again.');
            },
            complete: () => {
                this.isLoading = false;
                $button.prop('disabled', false);
                
                // Restore button state
                if ($button.find('i').length > 0 && originalIcon) {
                    $button.find('i').attr('class', originalIcon);
                } else {
                    $button.text(originalText);
                }
            }
        });
    }

    updateBasketCount() {
        $.ajax({
            url: '/Basket/GetBasketCount',
            type: 'GET',
            success: (response) => {
                console.log('Basket count updated:', response.count);
                $('.notification, .cart-count, .basket-count').text(response.count);
                if (response.count > 0) {
                    $('.notification, .cart-count, .basket-count').show();
                } else {
                    $('.notification, .cart-count, .basket-count').hide();
                }
            },
            error: () => {
                console.error('Error updating basket count');
            }
        });
    }

    updateMiniCart() {
        $.ajax({
            url: '/Basket/GetBasketItems',
            type: 'GET',
            success: (response) => {
                console.log('Mini cart updated:', response);
                if (response.success) {
                    this.renderMiniCart(response.items, response.total);
                }
            },
            error: () => {
                console.error('Error updating mini cart');
            }
        });
    }

    renderMiniCart(items, total) {
        const $wrapper = $('.minicart-item-wrapper ul');
        const $totalElement = $('.minicart-pricing-box .total strong');

        if (!$wrapper.length) {
            console.warn('Mini cart wrapper not found');
            return;
        }

        if (items.length === 0) {
            $wrapper.html('<li><p class="text-center" style="padding: 20px; color: #999;">Your cart is empty</p></li>');
            $totalElement.text('$0.00');
            return;
        }

        let html = '';
        items.forEach(item => {
            // Fix image path - use proper URL without Razor syntax
            const imageUrl = `/uploads/products/${item.imageUrl}` ;
            
            html += `
                <li class="minicart-item">
                    <div class="minicart-thumb">
                        <a href="#">
                            <img src="${imageUrl}" alt="${item.productName}" style="width: 50px; height: 50px; object-fit: cover;" />
                        </a>
                    </div>
                    <div class="minicart-content">
                        <h3 class="product-name">
                            <a href="#">${item.productName}</a>
                        </h3>
                        <p>
                            <span class="cart-quantity">${item.quantity} <strong>&times;</strong></span>
                            <span class="cart-price">${this.formatCurrency(item.price)}</span>
                        </p>
                        ${item.size || item.color ? `
                        <div class="product-details">
                            ${item.size ? `<span>Size: ${item.size}</span>` : ''}
                            ${item.color ? `<span>Color: ${item.color}</span>` : ''}
                        </div>` : ''}
                    </div>
                    <button class="minicart-remove" data-item-id="${item.id}" title="Remove item">
                        <i class="ion-android-close"></i>
                    </button>
                </li>
            `;
        });

        $wrapper.html(html);
        $totalElement.text(this.formatCurrency(total));

        // Bind remove events
        $('.minicart-remove').off('click').on('click', (e) => {
            e.preventDefault();
            const itemId = $(e.currentTarget).data('item-id');
            this.removeFromBasket(itemId);
        });
    }

    removeFromBasket(itemId) {
        if (!itemId) {
            this.showError('Invalid item ID');
            return;
        }

        $.ajax({
            url: '/Basket/RemoveItem',
            type: 'POST',
            data: { id: itemId },
            success: (response) => {
                if (response.success) {
                    this.showSuccess(response.message);
                    this.updateBasketCount();
                    this.updateMiniCart();
                    
                    // Remove from main cart page if we're on it
                    $(`tr[data-item-id="${itemId}"], .cart-item[data-item-id="${itemId}"]`).fadeOut();
                } else {
                    this.showError(response.message);
                }
            },
            error: () => {
                this.showError('Error removing item from basket');
            }
        });
    }

    toggleMiniCart() {
        $('.offcanvas-minicart-wrapper').toggleClass('active');
        if ($('.offcanvas-minicart-wrapper').hasClass('active')) {
            this.updateMiniCart();
            $('body').addClass('minicart-open');
        } else {
            $('body').removeClass('minicart-open');
        }
    }
    
    closeMiniCart() {
        $('.offcanvas-minicart-wrapper').removeClass('active');
        $('body').removeClass('minicart-open');
    }

    checkTransferBasket() {
        // Check if we need to transfer basket after login
        if (window.transferBasket === true) {
            console.log('Transferring basket after login...');
            $.ajax({
                url: '/Basket/TransferBasketOnLogin',
                type: 'POST',
                success: () => {
                    console.log('Basket transferred successfully');
                    this.updateBasketCount();
                    this.updateMiniCart();
                    window.transferBasket = false; // Reset flag
                },
                error: () => {
                    console.warn('Failed to transfer basket after login');
                }
            });
        }
    }

    highlightAddedItem(basketItem) {
        // Add visual feedback for successfully added item
        $('.notification, .cart-count, .basket-count').addClass('pulse-animation');
        setTimeout(() => {
            $('.notification, .cart-count, .basket-count').removeClass('pulse-animation');
        }, 1000);
    }

    formatCurrency(amount) {
        return new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD'
        }).format(amount || 0);
    }

    showSuccess(message) {
        console.log('Success:', message);
        if (typeof toastr !== 'undefined') {
            toastr.success(message);
        } else if (typeof Swal !== 'undefined') {
            Swal.fire({
                icon: 'success',
                title: 'Success!',
                text: message,
                timer: 3000,
                showConfirmButton: false,
                toast: true,
                position: 'top-end'
            });
        } else {
            // Fallback to alert
            alert(message);
        }
    }

    showError(message) {
        console.error('Error:', message);
        if (typeof toastr !== 'undefined') {
            toastr.error(message);
        } else if (typeof Swal !== 'undefined') {
            Swal.fire({
                icon: 'error',
                title: 'Error!',
                text: message
            });
        } else {
            // Fallback to alert
            alert(message);
        }
    }
}

// Initialize basket manager when document is ready
$(document).ready(function() {
    console.log('Initializing BasketManager...');
    window.basketManager = new BasketManager();
});

// Enhanced quantity controls for product detail pages
$(document).ready(function() {
    // Initialize quantity controls
    $('.pro-qty').each(function() {
        if (!$(this).find('.qtybtn').length) {
            $(this).prepend('<span class="dec qtybtn">-</span>');
            $(this).append('<span class="inc qtybtn">+</span>');
        }
    });

    // Handle quantity button clicks
    $(document).on('click', '.qtybtn', function() {
        var $button = $(this);
        var $input = $button.parent().find('input');
        var oldValue = parseInt($input.val()) || 1;
        var maxValue = parseInt($input.attr('max')) || 999;
        var minValue = parseInt($input.attr('min')) || 1;

        var newVal;
        if ($button.hasClass('inc')) {
            newVal = oldValue + 1;
            if (newVal > maxValue) {
                newVal = maxValue;
                window.basketManager?.showError(`Maximum available quantity is ${maxValue}`);
            }
        } else {
            newVal = oldValue - 1;
            if (newVal < minValue) {
                newVal = minValue;
            }
        }
        
        $input.val(newVal).trigger('change');
    });

    // Handle direct input changes
    $(document).on('change blur', '.pro-qty input, .quantity-input', function() {
        var $input = $(this);
        var value = parseInt($input.val()) || 1;
        var maxValue = parseInt($input.attr('max')) || 999;
        var minValue = parseInt($input.attr('min')) || 1;

        if (value > maxValue) {
            $input.val(maxValue);
            window.basketManager?.showError(`Maximum available quantity is ${maxValue}`);
        } else if (value < minValue) {
            $input.val(minValue);
        }
    });
});
