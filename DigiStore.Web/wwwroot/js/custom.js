function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'facebook',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000'
    });
    setTimeout(function () {
        location.reload();
    }, 3000);

}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}


function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }
});

$('#orderFilter').on('change',
    function () {
        $("#filter-forms").submit();
    });
function FillPageId(pageId) {
    console.log(pageId);
    $("#PageId").val(pageId);
    $("#filter-forms").submit();
}



$('[ajax-url-button]').on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');
    var itemId = $(this).attr('ajax-url-button');
    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "بله",
        cancelButtonText: "خیر",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((result) => {
        if (result.value) {
            $.get(url).then(result => {
                if (result.status === 'Success') {
                    ShowMessage('موفقیت', result.message);
                    $('#ajax-url-item-' + itemId).hide(1500);
                }
            });
        } else if (result.dismiss === swal.DismissReason.cancel) {
            swal('اعلام', 'عملیات لغو شد', 'error');
        }
    });
});

function OnSuccessRejectItem(res) {

    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "بله",
        cancelButtonText: "خیر",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((result) => {
        if (result.value) {
            if (res.status === 'Success') {
                ShowMessage('موفقیت', res.message);
                $('#ajax-url-item-' + res.data.Id).hide(1500);
                $('.close').click();

            }
        } else if (res.dismiss === swal.DismissReason.cancel) {
            swal('اعلام', 'عملیات لغو شد', 'error');
        }
    });
}


$("[Main_Category_Chekbox]").on('change',
    function (e) {
        var isChecked = ($(this).is(':checked'));
        var selectedCategoryId = $(this).attr('Main_Category_Chekbox');
        if (isChecked) {
            $('#sub_categories_' + selectedCategoryId).slideDown(300);
        } else {
            $('#sub_categories_' + selectedCategoryId).hide(300);
            $('[parent-category-id="' + selectedCategoryId + '"]').prop('checked', false);
        }
    });


$('#addColorButton').on('click',
    (e) => {
        e.preventDefault();
        var colorName = $('#Product_color_Name_input').val();
        var colorPrice = $('#Product_color_Price_input').val();
        if (colorName !== '' && colorPrice !== '') {
            var currentColorCount = $("#list_of_product_colors tr");
            var index = currentColorCount.length;


            var isExistsSelectedColor = $('[color-name-hidden-input][value="' + colorName + '"]');
            if (isExistsSelectedColor.length === 0) {

                var colorNameNode =
                    `<input type="hidden" value="${colorName}" name="ProductColors[${index
                    }].ColorCode"  color-name-hidden-input="${colorName}-${colorPrice}" />`;
                var colorPriceNode =
                    `<input type="hidden" value="${colorPrice}" name="ProductColors[${index
                    }].Price" color-Price-hidden-input="${colorName}-${colorPrice}" />`;

                $('#create_product_form').append(colorNameNode);
                $('#create_product_form').append(colorPriceNode);

                var colorNameTableNode =
                    `<tr color-table-item="${colorName}-${colorPrice}"> <td style="background-color:${colorName}">${colorName}</td> <td>${colorPrice
                    }</td> <td><a class="btn btn-danger" onclick="RemoveProductColor('${colorName}-${colorPrice
                    }')">حذف</a></td> </tr>`;
                $("#list_of_product_colors").append(colorNameTableNode);

                $('#Product_color_Name_input').val('');
                $('#Product_color_Price_input').val('');
            } else {
                ShowMessage('اخطار', 'رنگ وارد شده تکراری می باشد', 'warning');
                $('#Product_color_Name_input').val('').focus();
                $('#Product_color_Price_input').val('').focus();
            }

        } else {
            ShowMessage('اخطار', 'لطفا نام رنگ و قیمت آن را به درستی وارد نمایید', 'warning');
        }

    });

function reOrderProductFeaturesHiddenInputs() {
    var hiddenFeature = $('[feature-Name-hidden-input]');
    $.each(hiddenFeature,
        function (index, value) {
            var hiddenFeature = $(value);
            var FeatureId = $(value).attr('feature-Name-hidden-input');
            var hiddenValue = $('[feature-value-hidden-input="' + FeatureId + '"]');
            $(hiddenFeature).attr('name', 'ProductFeature[' + index + '].FeatureTitle');
            $(hiddenValue).attr('name', 'ProductFeature[' + index + '].FeatureValue');
        });
}

$('#addfeatureButton').on('click',
    (e) => {
        e.preventDefault();
        var featureName = $('#Product_feature_Name_input').val();
        var featureValue = $('#Product_feature_value_input').val();
        if (featureName !== '' && featureValue !== '') {
            var currentfeatureCount = $("#list_of_product_features tr");
            var index = currentfeatureCount.length;
            var isExistsSelectedfeature = $('[feature-Name-hidden-input][value="' + featureName + '"]');
            if (isExistsSelectedfeature.length === 0) {


                var featureNameNode =
                    `<input type="hidden" value="${featureName}" name="ProductFeature[${index
                    }].FeatureTitle" feature-Name-hidden-input="${featureName}-${featureValue}" />`;
                var featureValueNode =
                    `<input type="hidden" value="${featureValue}" name="ProductFeature[${index
                    }].FeatureValue" feature-value-hidden-input="${featureName}-${featureValue}" />`;

                $('#create_product_form').append(featureNameNode);
                $('#create_product_form').append(featureValueNode);

                var featureNameTableNode =
                    `<tr featureName-table-item="${featureName}-${featureValue}"> <td>${featureName}</td> <td>${featureValue
                    }</td> <td><a class="btn btn-danger" onclick="RemoveProductfeatures('${featureName}-${featureValue}')">حذف</a></td> </tr>`;
                $("#list_of_product_features").append(featureNameTableNode);

                $('#Product_feature_Name_input').val('');
                $('#Product_feature_value_input').val('');
            } else {
                ShowMessage('اخطار', 'ویژگی وارد شده تکراری می باشد', 'warning');
                $('#Product_feature_Name_input').val('').focus();
                $('#Product_feature_value_input').val('').focus();
            }
        }
        else {
            ShowMessage('اخطار', 'لطفا نام ویژگی و مقدار آن را به درستی وارد نمایید', 'warning');
        }

    });




function RemoveProductColor(index) {
    $('[color-name-hidden-input="' + index + '"]').remove();
    $('[color-Price-hidden-input="' + index + '"]').remove();
    $('[color-table-item="' + index + '"]').remove();
    reOrderProductColorHiddenInputs();

}

function RemoveProductfeatures(index) {
    $('[feature-Name-hidden-input="' + index + '"]').remove();
    $('[feature-value-hidden-input="' + index + '"]').remove();
    $('[featureName-table-item="' + index + '"]').remove();
    reOrderProductFeaturesHiddenInputs();

}

function reOrderProductGuaranteeHiddenInputs() {
    var hiddenColors = $('[guarantee-Name-hidden-input]');
    $.each(hiddenColors,
        function (index, value) {
            var hiddenColor = $(value);
            var guaranteeId = $(value).attr('guarantee-Name-hidden-input');
            var hiddenPrice = $('[guarantee-Price-hidden-input="' + guaranteeId + '"]');
            $(hiddenColor).attr('name', 'ProductGuarantee[' + index + '].GuaranteeName');
            $(hiddenPrice).attr('name', 'ProductGuarantee[' + index + '].Price');
        });
}
function reOrderProductColorHiddenInputs() {
    var hiddenColors = $('[color-name-hidden-input]');
    $.each(hiddenColors,
        function (index, value) {
            var hiddenColor = $(value);
            var colorId = $(value).attr('color-name-hidden-input');
            var hiddenPrice = $('[color-Price-hidden-input="' + colorId + '"]');
            $(hiddenColor).attr('name', 'ProductColors[' + index + '].ColorCode');
            $(hiddenPrice).attr('name', 'ProductColors[' + index + '].Price');
        });
}


$(document).ready(function () {
    var productId = $('#ProductVisited').attr('ProductVisite');
    $.ajax({
        url: '/visited/' + productId,
        type: "Get"
    });

});

function changeProductPriceBaseOnColor(ColorId, price) {
    var basePrice = parseInt($('#productBasePrice').val());
    var sum = (basePrice + price);
    $('.current_Price').html(sum);
    $('#add_product_to_order_ProductColorId').val(ColorId);

}

$('#number_of_Products_in_Order').on('change',
    function (e) {
        

    });


$('#number_of_Products_in_Order').on('change',
    function (e) {
        var numberOfProduct = parseInt(e.target.value, 0);
        $('#add_product_to_order_Count').val(numberOfProduct);
    });

$('#submitToOrderForm').on('click',
    function () {
        $('#addProductToOrdrForm').submit();
        open_waiting();

    });

function onSuccessAddProductToOrder(res) {
    if (res.status === 'Success') {
        ShowMessage('اعلان', res.message, res);

    } else {
        ShowMessage('اعلان', res.message, 'danger');
    }
    setTimeout(function () {
        close_waiting();
    }, 3000);
}

function RemoveProductFromOrder(detailId) {
    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "بله",
        cancelButtonText: "خیر",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((result) => {
        if (result.value) {
            $.get("/UserPanel/remove-order-item/" + detailId).then(res => {
                if (res.status === 'Success') {
                    open_waiting();
                    ShowMessage('اعلان', res.message, res);
                } else {
                    ShowMessage('اعلان', res.message, 'danger');
                }
                setTimeout(function () {
                    close_waiting();
                }, 3000);

            });
        } else if (res.dismiss === swal.DismissReason.cancel) {
            swal('اعلام', 'عملیات لغو شد', 'error');
        }
    });
}

function ChangeOpenOrderDetialCount(detialId, event) {
    $.get("/UserPanel/change-detialCount/" + detialId + "/" + event.target.value).then(res => {
        location.reload();
    });

}


$('#AddProductToFavorit').on('click',
    function () {
        $('#addProductFavorite').submit();
        open_waiting();

    });

function onSuccessAddProductFavorite(res) {
    if (res.status === 'Success') {
        ShowMessage('اعلان', res.message, res);

    } else {
        ShowMessage('اعلان', res.message, 'danger');
    }
    setTimeout(function () {
        close_waiting();
    }, 3000);
}

function RemoveProductFromFavoritList(favoritId, productId) {
    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "بله",
        cancelButtonText: "خیر",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((result) => {
        if (result.value) {
            $.get("/UserPanel/delete-product-favorit/" + favoritId + "/" + productId).then(res => {
                if (res.status === 'Success') {
                    open_waiting();
                    ShowMessage('اعلان', res.message, res);
                } else {
                    ShowMessage('اعلان', res.message, 'danger');
                }
                setTimeout(function () {
                    close_waiting();
                }, 3000);

            });
        } else if (res.dismiss === swal.DismissReason.cancel) {
            swal('اعلام', 'عملیات لغو شد', 'error');
        }
    });
}


