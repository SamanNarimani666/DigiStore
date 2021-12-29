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
        $("#filter-form").submit();
    });
function FillPageId(pageId) {
    console.log(pageId);
    $("#PageId").val(pageId);
    $("#filter-form").submit();
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
$('#addGuaranteeButton').on('click',
    (e) => {
        e.preventDefault();
        var guaranteeName = $('#Product_Guarantee_Name_input').val();
        var guaranteePrice = $('#Product_Guarantee_Price_input').val();
        if (guaranteeName !== '' && guaranteePrice !== '') {
            var currentGuaranteeCount = $("#list_of_product_guarantee tr");
            var index = currentGuaranteeCount.length;
            console.log(index);
            var guaranteeNameNameNode =
                `<input type="hidden" value="${guaranteeName}" name="ProductGuarantee[${index
                }].GuaranteeName" guarantee-Name-hidden-input="${guaranteeName}-${guaranteePrice}" />`;
            var guaranteePriceNode =
                `<input type="hidden" value="${guaranteePrice}" name="ProductGuarantee[${index
                }].Price" guarantee-Price-hidden-input="${guaranteeName}-${guaranteePrice}" />`;

            $('#create_product_form').append(guaranteeNameNameNode);
            $('#create_product_form').append(guaranteePriceNode);

            var guaranteeNameTableNode =
                `<tr Guarantee-table-item="${guaranteeName}-${guaranteePrice}"> <td>${guaranteeName}</td> <td>${guaranteePrice
                }</td> <td><a class="btn btn-danger" onclick="RemoveProductGuarantee('${guaranteeName}-${guaranteePrice}')">حذف</a></td> </tr>`;
            $("#list_of_product_guarantee").append(guaranteeNameTableNode);

            $('#Product_Guarantee_Name_input').val('');
            $('#Product_Guarantee_Price_input').val('');
        } else {
            ShowMessage('اخطار', 'لطفا نام رنگ و قیمت آن را به درستی وارد نمایید', 'warning');
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
                    }].ColorName"  color-name-hidden-input="${colorName}-${colorPrice}" />`;
                var colorPriceNode =
                    `<input type="hidden" value="${colorPrice}" name="ProductColors[${index
                    }].Price" color-Price-hidden-input="${colorName}-${colorPrice}" />`;

                $('#create_product_form').append(colorNameNode);
                $('#create_product_form').append(colorPriceNode);

                var colorNameTableNode =
                    `<tr color-table-item="${colorName}-${colorPrice}"> <td>${colorName}</td> <td>${colorPrice
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

function RemoveProductGuarantee(index) {
    $('[guarantee-Name-hidden-input="' + index + '"]').remove();
    $('[guarantee-Price-hidden-input="' + index + '"]').remove();
    $('[Guarantee-table-item="' + index + '"]').remove();
    reOrderProductGuaranteeHiddenInputs();
}

function RemoveProductColor(index) {
    $('[color-name-hidden-input="' + index + '"]').remove();
    $('[color-Price-hidden-input="' + index + '"]').remove();
    $('[color-table-item="' + index + '"]').remove();
    reOrderProductColorHiddenInputs();

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
            $(hiddenColor).attr('name', 'ProductColors[' + index + '].ColorName')
            $(hiddenPrice).attr('name', 'ProductColors[' + index + '].Price');
        });
}
