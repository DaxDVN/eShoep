﻿$(document).ready(function () {
  $('.addToCartButton').on('click', function (e) {
    e.preventDefault();

    const productId = $(this).closest('form').data('product-id');
    let qty = "1";
    const sst = $('#sst');
    if (sst.length) {
      qty = sst.val();
      sst.val(1);
    }

    $('#loadingModal').modal('show');

    $.ajax({
      url: '/ShoppingCart?handler=AddToCart',
      type: 'POST',
      contentType: "application/json",
      data: JSON.stringify({ProductId: productId, Qty: parseInt(qty)}),
      beforeSend: function (xhr) {
        xhr.setRequestHeader("XSRF-TOKEN",
          $('input:hidden[name="__RequestVerificationToken"]').val());
      },
      success: function (response) {
        $('#loadingModal').modal('hide');

        if (response.success) {
          $('#addToCartModal').modal('show');
        } else {
          location.href = "/Login";
        }
      },
      error: function (xhr, status, error) {
        $('#loadingModal').modal('hide');

        console.error("Error:", error);
        alert("There was an error processing your request.");
      }
    });
  });

  $("#closeBtn").click(function () {
    $('#addToCartModal').modal('hide');
  });
});
