$(document).ready(function () {
    var employeeJs = new EmployeeJs();
    //dialog chi tiết khách hàng
    dialogDetail = $(".m-dialog").dialog({
        autoOpen: false,
        fluid: true,
        //height: 400,
        //width: '700px',
        minWidth: 800,
        resizable: true,
        modal: true,
        position: ({ my: "center", at: "center", of: window }),
    });

    //dialog cảnh báo khi thực hiện các hành động xóa dữ liệu
    dialogWarning = $(".show-pop-up-warning").dialog({
        autoOpen: false,
        fluid: true,
        //height: 400,
        //width: '700px',
        minWidth: 400,
        resizable: true,
        modal: true,
        position: ({ my: "center", at: "center", of: window }),
    });
})

class EmployeeJs extends Base {
    constructor() {
        super();
        //  this.initEvent();
    }
    initEvent() {
        super.initEvent();
        var me = this;


        $(".search-table input[type=search],select[type=search],select[type=search]").on("blur", function () {
            debugger
            //this.setEndPoint();
            var searchText = $(".search-table input[type=search]").val();
            var departmentId = $(".search-table #searchDepartment").val();
            var positionId = $(".search-table #searchPosition").val();

            var query = "";
            var queryString = query.concat("/filter?searchText=", searchText, "&departmentId=", departmentId, "&positionId=", positionId);
            me.endPoint = queryString;
            me.reLoadData();
        })
    }
    /**
     * Hàm load lại dữ liệu
     * CreatedBy HNANH(3/12/2020)
     * */
    reLoadData() {
        super.loadData();
    }
    setEndPoint() {
        //debugger
    }
    setApiRouter() {
        this.apiRouter = '/api/v1/employees';
    }
    setObjName() {
        this.objName = "Employee";
    }

}