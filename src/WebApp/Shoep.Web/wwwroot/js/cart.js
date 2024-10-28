$(document).ready(function () {
  $('.addToCartButton').on('click', function (e) { // Thay # thành .
    e.preventDefault();

    const productId = $('#add_cart_form').data('product-id');
    let qty = "1";
    const sst = $('#sst');
    if (sst.length) {
      qty = sst.val();
    }
    $.ajax({
      url: '/Cart?handler=AddToCart',
      type: 'POST',
      contentType: "application/json",
      data: JSON.stringify({ProductId: productId, Qty: parseInt(qty)}),
      beforeSend: function (xhr) {
        xhr.setRequestHeader("XSRF-TOKEN",
          $('input:hidden[name="__RequestVerificationToken"]').val());
      },
      success: function (response) {
        if (response.success) {
          $('#addToCartModal').modal('show');
        } else {
          alert("Failed to add to cart.");
        }
      },
      error: function (xhr, status, error) {
        console.error("Error:", error);
        alert("There was an error processing your request.");
      }
    });
  });
  
  $("#closeBtn").click(function () {
    $('#addToCartModal').modal('hide');
  })
});
