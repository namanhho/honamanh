

class Base {
    constructor() {
        this.host = "http://localhost:51261";
        this.apiRouter = null;
        this.setApiRouter();
        this.objName = null;
        this.endPoint = "";
        this.setEndPoint();
        this.setObjName();
        this.initEvent();
        this.loadData();
        this.loadComboboxData();
    }
    setEndPoint() {

    }
    setApiRouter() {

    }
    setObjName() {

    }
    initEvent() {
        var me = this;
        //#region "Sự kiện với các nút" // dùng region để gộp các đoạn code giúp dễ quản lý và sửa đổi  
        //sự kiện click khi nhấn vào thêm mới
        $("#btn-add").click(this.btnAddOnClick.bind(this));

        //sự kiện click khi nhấn vào sửa
        $(".btn-edit").click(this.btnEditOnClick.bind(this));

        //load lại dữ liệu khi nhấn button nạp
        $("#btnRefresh").click(function () {
            me.loadData();
        });

        //Ẩn form chi tiết khi ấn hủy
        $("#btnCancel").click(function () {
            dialogDetail.dialog('close');
        });

        //Thực hiện lưu dữ liệu khi ấn button lưu
        $("#btnSave").click(this.btnSaveOnClick.bind(this));

        //Thực hiện xóa dữ liệu khi ấn button xóa
        $('.btn-delete').click(this.btnDeleteOnClick.bind(this));

        // sự kiện click vào button hủy để tắt cảnh báo
        $(".btn-cancel-warning").click(function () {
            dialogWarning.dialog("close");
        })

        // Sự kiện khi click vào button xóa để xác nhận thực hiện xóa
        $(".btn-accept-warning").click(function () {
            try {
                var tr = $('table tbody tr.rowSelected');
                var recordId = tr.data("recordid");
                $.ajax({
                    url: me.host + me.apiRouter + `/${recordId}`,
                    method: "Delete",
                }).done(function (res) {

                    // đóng form xác nhận xóa
                    dialogWarning.dialog("close");

                    //load lại dữ liệu
                    me.loadData();
                    var mess = res.Messenger;
                    //hiện thị thông báo xóa thành công
                    me.openPopUpMessenger("success", mess);
                }).fail(function (res) {
                    var mess = res.Messenger;
                    me.openPopUpMessenger("danger", mess);
                    dialogWarning.dialog("close");
                    debugger;
                })
            } catch (e) {
                me.openPopUpMessenger("danger", "Thực hiện lỗi, vui lòng kiểm tra lại");
                debugger
            }
        })


        // Sự kiện khi người dùng nhập dữ liệu tiền tệ
        $(".input-money").on("keyup", function (event) {


            // When user select text in the document, also abort.
            var selection = window.getSelection().toString();
            if (selection !== '') {
                return;
            }

            // When the arrow keys are pressed, abort.
            if ($.inArray(event.keyCode, [38, 40, 37, 39]) !== -1) {
                return;
            }


            var $this = $(this);

            // Get the value.
            var input = $this.val();

            var input = input.replace(/[\D\s\._\-]+/g, "");
            input = input ? parseInt(input, 10) : 0;

            $this.val(function () {
                return (input === 0) ? "" : input.toLocaleString("vi-VN");
            });
        });
        //#endregion Dialog

        //#region "Sự kiện với chuột"

        //Hiển thị thông tin chi tiết khi nhấn đúp chuột chọn 1 dòng trong bảng
        $("table tbody").on('dblclick', 'tr', this.btnEditOnClick.bind(this));

        // Hiển thị dòng trong bảng đã được chọn khi click chuột
        $("table tbody").on("click", 'tr', function () {
            $(this).siblings().removeClass("rowSelected");
            $(this).addClass("rowSelected");
        })

        //sự kiện click khi ấn nút chuyển trang
        $(".button-bottom-bar").focus(this.btnChangePageOnFocus);
        //#endregion "Sự kiện với chuột"

        //#region "Validate Kiểm tra input"
        /*
         * Validate bắt buộc nhập
         *CreatedBy: HNANH (14/11/2020)
         * */
        $('input[required]').blur(function () {
            //kiểm tra dữ liệu đã nhập, nếu để trống thì cảnh báo
            var value = $(this).val().trim();  //.trim() để đảm bảo khi người dùng nhập toàn dấu cách thì value vẫn sẽ là null;
            if (!value) {
                $(this).addClass("border-red");
                $(this).attr("title", "Trường này không được để trống");
                $(this).attr("validate", false);
            }
            else {
                $(this).removeClass("border-red");
                $(this).removeAttr("title");
                $(this).attr("validate", true);
            }
        });


        /*
         * Validate email nhập đúng định dạng
         *CreatedBy: HNANH (14/11/2020)
         * */
        $('input[type="email"]').blur(function () {
            var value = $(this).val().trim();
            var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!regex.test(value)) {
                $(this).addClass("border-red");
                $(this).attr("title", "Email không đúng định dạng");
                $(this).attr("validate", false);
            }
            else {
                $(this).removeClass("border-red");
                $(this).attr("validate", true);
            }
        });

        /*
         Kiểm tra thông tin mã đã đúng định dạng chưa
         CreatedBy HNANH (2/12/2020)
         */
        $('input[codeAuto]').blur(function () {
            //kiểm tra dữ liệu đã nhập, nếu để trống thì cảnh báo
            var value = $(this).val().trim();  //.trim() để đảm bảo khi người dùng nhập toàn dấu cách thì value vẫn sẽ là null;
            var check = /NV-[0-9]{1,10}/.test(value);

            //Kiểm tra dữ liệu có đúng định dạng chưa
            if (!check) {
                $(this).addClass("border-red");
                $(this).attr("title", "Dữ liệu không đúng định dạng (NV-xxxxxxxxxx)");
                $(this).attr("validate", false);
            }
            else {
                $(this).removeClass("border-red");
                $(this).removeAttr("title");
                $(this).attr("validate", true);
            }
        });

        $(".search-table select[type=search]").on("change", function () {
            var selects = $(".search-table select[type=search]");
            $.each(selects, function (index, select) {

            })
        })

        //#endregion "Validate Kiểm tra input"
    }

    /**Load dữ liệu
    * CreatedBy: HNANH (12/11/2020)
    * */
    loadData() {
        var me = this;
        try {
            //debugger
            var entityId = me.objName + "Id";
            $(".loading").show();
            //xóa hết dữ liệu bảng trước khi nạp, tránh bị nạp tiếp vào dữ liệu đã có
            $('table tbody').empty();
            //lấy thông tin các cột dữ liệu
            var ths = $("table thead th");

            //Lấy thông tin dữ liệu sẽ map tương ứng với các cột
            $.ajax({
                url: this.host + this.apiRouter + this.endPoint,
                method: "GET",
            }).done(function (res) {
                if (res.length == 0) {
                    $(".loading").hide();
                    me.openPopUpMessenger("info", "Dữ liệu trống");
                    return;
                }
                $.each(res, function (index, obj) {
                    var tr = $(`<tr></tr>`);
                    tr.data("recordid", obj[entityId]);
                    $.each(ths, function (index, th) {
                        var td = $(`<td></td>`);

                        var fieldName = $(th).attr("fieldName");
                        var value = obj[fieldName];
                        var formatType = $(th).attr("formatType");
                        switch (formatType) {
                            case "ddmmyyyy":
                                value = formatDate(value);
                                td.addClass("align-center");
                                break;
                            case "money":
                                value = formatMoney(value);
                                td.addClass("align-right");
                                break;
                            case "address":
                                td.addClass("fix-width-table align-money");
                                $(".fix-width-table").attr("title", value);
                            default:
                        }
                        //  debugger
                        $(td).append(value);
                        $(tr).append(td);
                    });
                    $("table tbody").append(tr);
                });

                // tắt icon loading khi dữ liệu đã load xong
                $(".loading").hide();

            }).fail(function (res) {
                $(".loading").show();
                me.openPopUpMessenger("danger", "Thực hiện lỗi, vui lòng kiểm tra lại");
            })
        } catch (e) {
            console.log(e);
        }
    }

    /**
     * Hàm xử lý sự kiện khi nhấn button thêm
     * CreatedBy: HNANH (18/11/2020)
     * */
    btnAddOnClick() {
        var me = this;
        try {
            me.FormMode = "Add";
            dialogDetail.dialog('open');

            // Xử lý làm trống các thành phần input khi mở lại form
            var inputs = $(".m-dialog-content input, .m-dialog-content select");
            $("input").removeClass("border-red");
            $.each(inputs, function (index, input) {
                if ($(this).attr("type") == "radio") {
                    if ($(this).attr("checked")) {
                        $(this).prop("checked", true);
                    }
                }
                else {
                    $(this).val("");
                }
            })

            //load dữ liệu cho các combobox 
            var selects = $('.m-dialog select[selectName]');

            //xử lý xóa các option trước để tránh bị trùng khi nhấn button add các lần tiếp theo
            //$('select option').remove();
            selects.empty();

            //hiện thị icon load khi dữ liệu đang được tải
            $(".loading").show();

            $.each(selects, function (index, select) {
                var api = $(this).attr("api");
                var fieldName = $(this).attr("selectName");
                var fieldValue = $(this).attr("selectValue");

                //lấy dữ liệu nhóm khách hàng hoặc phòng ban, vị trí/chức vụ
                $.ajax({
                    url: me.host + api,
                    method: "GET",
                }).done(function (res) {
                    if (res) {
                        $.each(res, function (index, obj) {
                            var option = $(`<option value=` + obj[fieldName] + `>` + obj[fieldValue] + `</option>`)
                            $(select).append(option);
                        })
                    }
                    $(".loading").hide();
                    debugger
                }).fail(function (res) {
                    $(".loading").hide();
                    me.openPopUpMessenger("danger", "Thực hiện lỗi, vui lòng kiểm tra lại");
                    debugger
                })
            })

            //Lấy danh sách nhân viên để tiến hành tự động điền mã nhân viên tăng lên 1
            //debugger
            var inputs = $("input[codeAuto]");
            $.each(inputs, function (index, input) {
                var api = $(this).attr("api");
                //lấy dữ liệu nhóm khách hàng
                $.ajax({
                    url: me.host + me.apiRouter + api,
                    method: "GET",
                }).done(function (res) {
                    var entity = res[0];
                    var entityCode = "";
                    entityCode = entity["EmployeeCode"];
                    var length = entityCode.length;
                    var numberCode = entityCode.substring(3, length);
                    numberCode++;
                    var entityCodeAuto = "NV-" + numberCode;
                    $(input).val(entityCodeAuto);
                    $(".loading").hide();
                    debugger
                }).fail(function (res) {
                    $(".loading").hide();
                    me.openPopUpMessenger("danger", "Thực hiện lỗi, vui lòng kiểm tra lại");
                    debugger
                })

            })
        } catch (e) {
            console.log(e);
        }
    }

    /**
     * Hàm xử lý sự kiện khi nhấn button lưu
     * CreatedBy: HNANH (18/11/2020)
     * */
    btnSaveOnClick() {
        try {
            var me = this;
            //Validate dữ liệu
            var inputValidate = $("input[required], input[type='email'], input[codeAuto]");
            $.each(inputValidate, function (index, input) {
                $(input).trigger('blur');
            });
            var inputNotValids = $('input[validate= false]');
            if (inputNotValids && inputNotValids.length > 0) {
                //alert("Dữ liệu không hợp lệ, vui lòng kiểm tra lại");
                me.openPopUpMessenger("warning", "Dữ liệu không hợp lệ, vui lòng kiểm tra lại");
                inputNotValids[0].focus();
                return;
            }

            //thu thập thông tin dữ liệu được nhập-> buil thành obj

            // lấy tất cả các control nhập liệu
            var inputs = $('input[fieldName], .m-dialog select[fieldName]');
            var entity = {};
            $.each(inputs, function (index, input) {
                var propertyName = $(input).attr('fieldName');
                var value = $(input).val();
                if ($(input).val()) {
                    value = $(input).val().trim();
                }

                //check với trường hợp là radio, thì chỉ lấy value của input có attribute là checked

                if ($(this).attr("type") == 'radio') {
                    if ($(this).is(":checked")) {
                        entity[propertyName] = value;
                        // debugger
                    }
                }
                else if ($(this).attr("typeName") == 'money') {
                    value = value.split('.').join('');
                    value = value.split(',').join('.');
                    if (value == "") {
                        entity[propertyName] = 0;
                    }
                    else {
                        var money = parseFloat(value);
                        entity[propertyName] = money;
                    }
                }
                else {
                    entity[propertyName] = value;
                }
            });
            //debugger;
            //Gọi sevice tương ứng thực hiện lưu dữ liệu
            var method = "POST";
            var endPoint = "";
            if (me.FormMode == "Edit") {
                method = "PUT";
                var entityId = me.objName + "Id";
                entity[entityId] = me.recordId;
                endPoint = me.recordId;
            }
            $.ajax({
                url: me.host + me.apiRouter + `/${endPoint}`,
                method: method,
                data: JSON.stringify(entity),
                contentType: 'application/json',
            }).done(function (res) {

                //Sau khi lưu thành công:

                //+ đưa ra thông báo
                var messenger = res.Messenger;

                me.openPopUpMessenger("success", messenger);

                //+ ẩn form
                dialogDetail.dialog('close');
                //+ load lại dữ liệu
                me.loadData();
            }).fail(function (res) {
                var respond = res.responseText;
                var objError = JSON.parse(respond);
                var errorData = objError["Data"][0];
                me.openPopUpMessenger("danger", errorData);
                debugger;
            })
        } catch (e) {
            console.log(e);
        }

    }
    /**
         * Hàm xử lý khi nhấn button sửa hoặc ấn dblclick
         * CreatedBy: HNANH (12/11/2020)
         * */
    btnEditOnClick() {
        try {
            var me = this;
            //load dữ liệu cho các combobox 
            var selects = $('.m-dialog select[selectName]');
            var api = selects.attr("api");
            $("input").removeClass("border-red");
            //xử lý xóa các option trước để tránh bị trùng khi nhấn button add các lần tiếp theo
            //$('select option').remove();
            selects.empty();

            //hiện thị icon load khi dữ liệu đang được tải
            $(".loading").show();
            $.each(selects, function (index, select) {
                var api = $(this).attr("api");
                var fieldName = $(this).attr("selectName");
                var fieldValue = $(this).attr("selectValue");

                //lấy dữ liệu nhóm khách hàng
                $.ajax({
                    url: me.host + api,
                    method: "GET",
                }).done(function (res) {
                    if (res) {
                        $.each(res, function (index, obj) {
                            var option = $(`<option value=` + obj[fieldName] + `>` + obj[fieldValue] + `</option>`)
                            $(select).append(option);
                        })
                    }
                    $(".loading").hide();
                }).fail(function (res) {
                    $(".loading").hide();
                    me.openPopUpMessenger("danger", "Thực hiện lỗi, vui lòng kiểm tra lại");
                    debugger
                })
            })

            me.FormMode = "Edit";
            var tr = $("table tbody tr.rowSelected");
            if (tr.html() == null) {
                me.openPopUpMessenger("info", "Vui lòng chọn bản ghi muốn sửa");
                return;
            }

            dialogDetail.dialog("open");
            //lấy thông tin khóa chính của bản ghi
            var recordId = tr.data('recordid');
            me.recordId = recordId;
            // var recordId = $(this).attr("recordId");

            //gọi service để lấy thông tin chi tiết
            $.ajax({
                url: me.host + me.apiRouter + `/${recordId}`,
                method: "GET",
            }).done(function (res) {

                //Build thành obj vào đẩy tương ứng vào form
                var datas = res;
                var inputs = $('.m-dialog input[fieldName], .m-dialog select[fieldName]');
                $.each(inputs, function (index, input) {
                    var propertyName = $(input).attr('fieldName');
                    var value = datas[propertyName];
                    //check với trường hợp là radio, thì chỉ lấy value của input có attribute là checked
                    if ($(this).attr("type") == 'date') {
                        var date = getDateStringYYYYMMDD(value);
                        $(this).val(date);
                    }
                    else if ($(this).attr("type") == 'radio') {
                        if ($(this).val() == value) {
                            $(this).prop("checked", true);
                            //   $("#male").prop("checked", false);
                        }
                        else {
                            $(this).prop("checked", false);
                        }
                    }
                    else if ($(this).attr("typeName") == 'money') {
                        var money = formatMoney(value);
                        $(this).val(money);
                    }
                    else {
                        $(this).val(value).change();
                    }
                });
            }).fail(function (res) {
                me.openPopUpMessenger("danger", "Thực hiện lỗi, vui lòng kiểm tra lại");
            })
        } catch (e) {
            console.log(e);
        }
    }

    /**
        * Hàm xử lý khi nhấn button xóa
        * CreatedBy: HNANH (12/11/2020)
        * */
    btnDeleteOnClick() {
        var me = this;
        //lấy thông tin bản ghi đã chọn trong danh sách
        var tr = $('table tbody tr.rowSelected');

        if (tr.html() == null) {
            me.openPopUpMessenger("info", "Vui lòng chọn bản ghi muốn xóa");
            return;
        }
        //lấy thông tin chi tiết của bản ghi đã chọn
        var recordId = tr.data("recordid");

        //mở dialog cảnh báo khi thực hiện xóa
        me.openDialogWarning(recordId);
    }

    /**
     * Hàm mở dialog thông báo: thành công, cảnh báo, lỗi,..
     * CreatedBy: HNANH (21/11/2020)
     * */
    openPopUpMessenger(status, messenger) {
        //   debugger;
        $(".toast-messenger .title-messenger").text(messenger);

        //Thực hiện remove các class được thêm trước đó
        var classes = $(".div-icon-messenger").attr("class").split(/\s+/);;
        var classesIconCloseMess = $(".btn-close-messenger .icon-close-messenger").attr("class").split(/\s+/);;
        var l = classes.length;
        for (var i = 1; i < l; i++) {
            // debugger
            $(".div-icon-messenger").removeClass(classes[i]);
            $(".btn-close-messenger .icon-close-messenger").removeClass(classesIconCloseMess[i]);
        }
        switch (status) {
            case "success":
                $(".div-icon-messenger").addClass("icon-messenger-success");
                $(".btn-close-messenger .icon-close-messenger").addClass("icon-close-messenger-success");
                break;
            case "danger":
                $(".div-icon-messenger").addClass("icon-messenger-danger");
                $(".btn-close-messenger .icon-close-messenger").addClass("icon-close-messenger-danger");
                break;
            case "warning":
                $(".div-icon-messenger").addClass("icon-messenger-warning");
                $(".btn-close-messenger .icon-close-messenger").addClass("icon-close-messenger-warning");
                break;
            case "info":
                $(".div-icon-messenger").addClass("icon-messenger-info");
                $(".btn-close-messenger .icon-close-messenger").addClass("icon-close-messenger-info");
                break;
            default:
        }
        $(".show-toast-messenger").css("visibility", "visible");
        setTimeout(function () {
            $(".show-toast-messenger").css("visibility", "hidden");
        }, 5000);

    }
    /**
     * Hàm mở dialog xác nhận
     * @param {any} recordId tham số là recordId: mã khách hàng muốn xóa
     * CreatedBy: HNANH (20/11/2020)
     */
    openDialogWarning(recordId) {
        try {
            var me = this;
            var recordId = recordId;
            $.ajax({
                url: me.host + me.apiRouter + `/${recordId}`,
                method: "GET",
            }).done(function (res) {
                var entityName = res["FullName"];
                var entityCode = res[`${me.objName}Code`];

                $(".body-pop-up .content-pop-up-warning").text("Bạn có chắc chắn muốn xóa khách hàng "
                    + entityName + " (Mã khách hàng " + entityCode + ") không ? ");
                $("span.ui-dialog-title").text('Xác nhận xóa bản ghi');
                dialogWarning.dialog("open");
                $(".m-seconds-button-2").blur();
            }).fail(function () {

            })
        } catch (e) {
            console.log(e);
        }
    }

    /**
     * Hàm xử lý đổi màu nút thay đổi trang
     * CreatedBy: HNANH (12/11/2020)
     * */
    btnChangePageOnFocus() {
        $(this).siblings().removeClass("button-change-page-select");
        $(this).addClass("button-change-page-select");
    }


    /**
     * Hàm lưu dữ liệu nhập từ dialog
     * CreatedBy: HNANH (12/11/2020)
     * */
    checkRequire() {
        debugger;
    }


    /**
     * Lấy dữ liệu combom box 
     * CreatedBy: HNANH (03/12/2020)
     * */
    loadComboboxData() {
        var me = this;
        //load dữ liệu cho các combobox 
        var selects = $('.search-table select[selectName]');
        //xử lý xóa các option trước để tránh bị trùng khi nhấn button add các lần tiếp theo
        //$('select option').remove();
        //selects.empty();
        $.each(selects, function (index, select1) {
            //debugger
            var optionNumber = select1.options.length;
            for (var i = 1; i < optionNumber; i++) {
                select1.options[i] = null;
            }
        })

        //hiện thị icon load khi dữ liệu đang được tải
        $(".loading").show();

        $.each(selects, function (index, select) {
            var api = $(this).attr("api");
            var fieldName = $(this).attr("selectName");
            var fieldValue = $(this).attr("selectValue");
            //lấy dữ liệu nhóm khách hàng
            $.ajax({
                url: me.host + api,
                method: "GET",
            }).done(function (res) {
                if (res) {
                    $.each(res, function (index, obj) {
                        var option = $(`<option value=` + obj[fieldName] + `>` + obj[fieldValue] + `</option>`)
                        $(select).append(option);
                    })
                }
                //$(".loading").hide();
                debugger
            }).fail(function (res) {
                $(".loading").hide();
                me.openPopUpMessenger("danger", "Thực hiện lỗi, vui lòng kiểm tra lại");
                debugger
            })
        })
    }

}