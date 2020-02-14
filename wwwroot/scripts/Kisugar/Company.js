$(document).ready(function() {
	App();
});
var App = function() {
	dataTable = $("#tablecompany").DataTable({
		bDestroy: true, //ทำรายอันเก่าทิ้งแล้วรอคำสั่ง reload
		ajax: {
			url: "http://192.168.10.46/sdapi/sdapi/companyget",
			dataSrc: ""
		},
		//"columnDefs": [{
		//    "targets": 0,
		//    "searchable": false,
		//    "className": 'check'
		//}
		//],
		columns: [
			//{ "defaultContent": "" },//  Set columns checkbox
			{
				data: "CompCode",
				width: "20px",
				orderable: false,
				className: "text-center",
				render: function(data, type) {
					return "<a class='fa fa-book btn' data-id='" + data + "'></a>";
				}
			},
			{
				width: "20px",
				orderable: false,
				className: "text-center",
				render: function(data, type) {
					return "<a class='fa fa-hand-o-up btn' onclick=DataBranch()></a>";
				}
			},
			{
				width: "20px",
				orderable: false,
				className: "text-center",
				render: function(data, type) {
					return "<a class='fa fa-globe btn' onclick=DataRegion()></a>";
				}
			},
			{
				title: "รหัส",
				width: "30px",
				data: "CompCode"
			},
			{
				title: "ชื่อบริษัท",
				data: "NameTH"
			},
			{
				data: "CompCode",
				render: function(data, type) {
					return (
						"<a class='btn btn-danger btn-sm bt btndelect' data-id='" +
						data +
						"'><i class='fa fa-pencil'></i> Delect</a>"
					);
				},

				orderable: false,
				searchable: false,
				width: "150px"
			}
		],

		//'select': {
		//    'style': 'multi'
		//},
		order: [[3, "asc"]],
		language: {
			emptyTable: "No data found, Please click on <b>Add New</b> Button",
			sSearch: "ค้นหา",
			sLengthMenu: "แสดงข้อมูลทีละ _MENU_ ข้อมูล",
			sInfo: "ข้อมูล _START_ ถึง _END_ จากทั้งหมด _TOTAL_ ข้อมูล",
			oPaginate: {
				sFirst: "หน้าแรก",
				sPrevious: "ก่อนหน้า",
				sNext: "ถัดไป",
				sLast: "หน้าสุดท้าย"
			}
		}
	});

	$("#tablecompany").on("click", ".btndelect", function(e) {
		swal({
			title: "แจ้งเตือน",
			text: "คุณต้องการลบรายการนี้ใช่หรือไม่",
			icon: "warning",
			buttons: ["ไม่แน่ใจ, ยกเลิก!", "แน่ใจ, ลบเดี๋ยวนี้!"],
			dangerMode: true
		}).then(willDelete => {
			if (willDelete) {
				var roleid = $(this).data("id");
				var url = "http://192.168.10.46/sdapi/sdapi/companydel/" + roleid;
				$.ajax({
					url: url,
					type: "POST"
				}).done(function(response) {
					if (response.Success) {
						swal("แจ้งเตือน", "ลบสำเร็จ", {
							icon: "success"
						});
						dataTable.ajax.reload();
					} else {
						toastr.error(msg.message, "แจ้งเตือน");
					}
				});
			} else {
			}
		});

		return false;
	});
	$("#tablecompany").on("click", ".fa-book", function(e) {
		$("#loginModal").modal("show");
		$("#Statusform").val("Edit");
		var roleid = $(this).data("id");
		var url = "http://192.168.10.46/sdapi/sdapi/companyget/" + roleid;
		$.get(url, function(data) {
			console.info(data);
			var DT = data;
			for (i = 0; i < DT.length; i++) {
				var rec = DT[i];
				$("#CompCode").val(rec.CompCode);
				$("#NameTH").val(rec.NameTH);
				$("#NameEN").val(rec.NameEN);
				$("#Addr1").val(rec.Addr1);
				$("#Addr2").val(rec.Addr2);
				$("#Addr3").val(rec.Addr3);
				$("#ZIP").val(rec.ZIP);
				$("#Tel").val(rec.Tel);
				$("#Fax").val(rec.Fax);
				$("#Email").val(rec.Email);
				$("#WebSite").val(rec.WebSite);
				$("#TaxID").val(rec.TaxID);
				$("#RegID").val(rec.RegID);
				$("#RegDate").val(rec.RegDate);
				$("#DistFrPlant").val(rec.DistFrPlant);
				$("#FisCompCode").val(rec.FisCompCode);
				$("#FisBranchCode").val(rec.FisBranchCode);
			}
		});
		return false;
	});
	$("#btnHideModal").on("click", () => {
		$("#loginModal").modal("hide");
	});
	$("#Btncm").on("click", () => {
		Updatecompany();
	});
	$("#Addnew").on("click", () => {
		$("#loginModal").modal("show");
		$("#formcm")[0].reset();
		$("#Statusform").val("Add");
	});
};

var Updatecompany = function Updatecompany() {
	var valdata = $("#formcm").serialize();
	$.ajax({
		url: "/Company/SaveCompany",
		type: "POST",
		dataType: "json",
		contentType: "application/x-www-form-urlencoded; charset=UTF-8", // this
		data: valdata,
		success: function(msg) {
			if (msg.success) {
				$("#loginModal").modal("hide");
				dataTable.ajax.reload();
				toastr.success(msg.message, "แจ้งเตือน");
			} else {
				$("#loginModal").modal("hide");
				toastr.error(msg.message, "แจ้งเตือน");
			}
		},
		error: function(errormessage) {
			//do something else
			toastr.error(errormessage, "แจ้งเตือน");
		}
	});
};
///-- COMPANY SCRIPT----------------------------------------------------------------------------------