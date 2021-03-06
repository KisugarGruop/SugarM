$(document).ready(function () {
	App();
	$(".branch").hide();
	$(".region").hide();
	$('.kt-select2').select2({
		placeholder: "Select a state"
	});

	$('[data-toggle="tooltip"]').tooltip();
});
var App = function () {
	var key = $("#KIKEY").val();
	dataTablecm = $("#tablecompany").DataTable({
		bDestroy: true, //ทำลายอันเก่าทิ้งแล้วรอคำสั่ง reload
		paging: true, //showpaging
		lengthChange: false,
		searching: false,
		ordering: true,
		info: true,
		autoWidth: false,
		ajax: {
			url: "/Company/Getcompany",
			type: "GET",
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
				width: "20px",
				orderable: false,
				className: "text-center",
				render: function (data, type) {
					return "<div class='kt-demo-icon__preview detailbranch' style='cursor: pointer' data-toggle='tooltip' data-placment='bottom' title='สาขา'><i class='fa fa-book-open'></i></div>";
				}
			},
			{
				width: "20px",
				orderable: false,
				className: "text-center",
				render: function (data, type) {
					return "<div class='kt-demo-icon__preview detailregion' style='cursor: pointer' data-toggle='tooltip' data-placment='bottom' title='เขต'><i class='fa fa-book-open'></i></div>";

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
				render: function (data, type) {
					return (
						"<a class='btn btn-primary btn-sm active detail' data-id='" +
						data +
						"'><font color='white'><i class='flaticon-folder-1'></i> Detail</a>  <a class='btn btn-danger btn-sm active btndelect' data-id='" +
						data +
						"'><i class='flaticon-delete-1'></i> Delete</a>"
					);
				},

				orderable: false,
				searchable: false,
				width: "180px"
			}
		],

		//'select': {
		//    'style': 'multi'
		//},
		order: [
			[3, "asc"]
		],
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
	//- ลบรายการ tablecompany
	$("#tablecompany").on("click", ".btndelect", function (e) {
		swal.fire({
			title: 'แจ้งเตือน',
			text: "คุณต้องการลบรายการนี้ใช่หรือไม่!",
			type: 'warning',
			showCancelButton: true,
			buttons: ["ไม่แน่ใจ, ยกเลิก!", "แน่ใจ, ลบเดี๋ยวนี้!"],
			dangerMode: true
		}).then(function (result) {
			if (result.value) {
				var roleid = $(this).data("id");
				var url = "/Company/Delectcompany/" + roleid;
				$.ajax({
					url: url,
					type: "POST"
				}).done(function (response) {
					if (response.success) {
						swal.fire(
							'แจ้งเตือน!',
							'ลบสำเร็จ.',
							'success'
						)
						dataTablecm.ajax.reload();
					} else {
						toastr.error(msg.message, "แจ้งเตือน");
					}
				});

			}
		});
		return false;
	});
	//- ลบรายการ tablebranch
	$("#tablebranch").on("click", ".btndelect", function (e) {
		swal({
			title: "แจ้งเตือน",
			text: "คุณต้องการลบรายการนี้ใช่หรือไม่",
			icon: "warning",
			buttons: ["ไม่แน่ใจ, ยกเลิก!", "แน่ใจ, ลบเดี๋ยวนี้!"],
			dangerMode: true
		}).then(willDelete => {
			if (willDelete) {
				var brancode = $(this).data("id");
				var roleid = $(this)
					.parent()
					.parent()
					.find(".fa-book")
					.data("id");
				var url = "/Company/Delectbanch/" + brancode + "/" + roleid;
				$.ajax({
					url: url,
					type: "POST"
				}).done(function (response) {
					if (response.success) {
						swal("แจ้งเตือน", "ลบสำเร็จ", {
							icon: "success"
						});
						dataTablebn.ajax.reload();
					} else {
						toastr.error(msg.message, "แจ้งเตือน");
					}
				});
			} else {}
		});

		return false;
	});
	//- โชว์รายละเอียด branch
	$("#tablecompany").on("click", ".detail", function (e) {
		$(".branch").hide();
		$(".region").hide();
		$("#kt_modal_4_2").modal("show");
		$("#Statusform").val("Edit");
		var roleid = $(this).data("id");
		var url = "/Company/Getcompany/" + roleid;
		$.ajax({
			url: url,
			data: {
				data: "data"
			},
			type: "GET",
			success: function (data) {
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
			}
		});
		return false;
	});
	//---- รายละเอียดbranch
	$("#tablecompany").on("click", ".detailbranch", function (e) {
		$(".company").hide();
		$(".region").hide();
		$(".branch").show();
		dataTablebn = $("#tablebranch").DataTable({
			bDestroy: true, //ทำลายอันเก่าทิ้งแล้วรอคำสั่ง reload
			ajax: {
				url: "/Company/Getcompanybanch",
				dataSrc: "",
				type: "GET"
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
					render: function (data, type) {
						return "<div class='kt-demo-icon__preview' style='cursor: pointer' data-toggle='tooltip' data-placment='bottom' title='แก้ไข' data-id='" + data + "'><i class='fa fa-folder-open'></i></div>";
					}
				},
				{
					title: "รหัสสาขา",
					width: "60px",
					data: "CompCode"
				},
				{
					title: "ชื่อ",
					data: "NameTH"
				},
				{
					title: "ประเภท",
					data: "BranchType"
				},
				{
					data: "BranchCode",
					render: function (data, type) {
						return (
							"<font color='white'><a class='btn btn-danger btn-sm btndelect' font-corlor='with' data-id='" +
							data +
							"'><i class='fa fa-pencil'></i> Delete</a>"
						);
					},

					orderable: false,
					searchable: false,
					width: "80px"
				}
			],

			//'select': {
			//    'style': 'multi'
			//},
			order: [
				[3, "asc"]
			],
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
	});
	//---- รายละเอียดcompanyregion
	$("#tablecompany").on("click", ".detailregion", function (e) {
		$(".company").hide();
		$(".companybranch").hide();
		$(".branch").hide();
		$(".region").show();
		dataTablebnregion = $("#tableregion").DataTable({
			bDestroy: true, //ทำลายอันเก่าทิ้งแล้วรอคำสั่ง reload
			ajax: {
				url: "/Company/Getcompanyregion",
				dataSrc: "",
				type: "GET"
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
					render: function (data, type) {
						return "<div class='kt-demo-icon__preview' style='cursor: pointer' data-toggle='tooltip' data-placment='bottom' title='แก้ไข' data-id='" + data + "'><i class='fa fa-folder-open'></i></div>";

					}
				},
				{
					title: "รหัสสาขา",
					width: "60px",
					data: "RegionCode"
				},
				{
					title: "ชื่อ",
					data: "NameTH"
				},
				{
					data: "RegionCode",
					render: function (data, type) {
						return (
							"<font color='white'><a class='btn btn-danger btn-sm btndelect' font-corlor='with' data-id='" +
							data +
							"'><i class='fa fa-pencil'></i> Delete</a>"
						);
					},

					orderable: false,
					searchable: false,
					width: "80px"
				}
			],

			//'select': {
			//    'style': 'multi'
			//},
			order: [
				[1, "asc"]
			],
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
	});
	//- รายละเอียดbranch ข้างใน
	$("#tablebranch").on("click", ".kt-demo-icon__preview", function (e) {
		$("#kt_modal_4_2").modal("show");
		$("#Statusform").val("Edit");
		var banchid = $(this).data("id");
		var comid = $(this)
			.parent()
			.parent()
			.find(".btndelect")
			.data("id");
		var url = "/Company/Getcompanybanch/" + banchid + "/" + comid;
		$.ajax({
			url: url,
			data: {
				data: "data"
			},
			type: "GET",
			success: function (data) {
				var DT = data;
				for (i = 0; i < DT.length; i++) {
					var rec = DT[i];
					$("#CompCode").val(rec.CompCode);
					$("#BranchCode").val(rec.BranchCode);
					$("#BranchType").val(rec.BranchType);
					$("#BranchTypeId").val(rec.BranchTypeId);
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
					$("#Head").val(rec.Head);
					$("#CfsUserId").val(rec.CfsUserId);
					$("#LeasingContractId").val(rec.LeasingContractId);
					$("#LoanContractId").val(rec.LoanContractId);
					$("#GuaranteeContractId").val(rec.GuaranteeContractId);
					$("#CaneContractId").val(rec.CaneContractId);
					debugger;
					updateElement();
				}
			}
		});
		dataTablebnregionbn = $("#tablebranchregion").DataTable({
			bDestroy: true, //ทำลายอันเก่าทิ้งแล้วรอคำสั่ง reload
			ajax: {
				url: "/Company/Getbanchregion/" + banchid + "/" + comid,
				dataSrc: "",
				type: "GET",
				dataType: "json"
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
					data: "ProvCode",
					width: "20px",
					orderable: false,
					className: "text-center",
					render: function (data, type) {
						return "<a class='fa fa-book btn' data-id='" + data + "'></a>";
					}
				},
				{
					title: "AmpCode",
					width: "60px",
					data: "AmpCode"
				},
				{
					title: "TumCode",
					data: "TumCode"
				},
				{
					title: "จังหวัด",
					data: "ProvName"
				},
				{
					title: "อำเภอ",
					data: "AmpName"
				},
				{
					title: "ตำบล",
					data: "TumName"
				},
				{
					title: "หมู่บ้าน",
					data: "VillName"
				}
			],

			//'select': {
			//    'style': 'multi'
			//},
			order: [
				[3, "asc"]
			],
			language: {
				emptyTable: "No data found",
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
		return false;
	});
	//- โชว์รายละเอียด region
	$("#tableregion").on("click", ".kt-demo-icon__preview", function (e) {
		$(".branch").hide();
		$(".company").hide();
		$(".companybranch").hide();

		$("#kt_modal_4_2").modal("show");
		$("#Statusform").val("Edit");
		var ComeCode = $(this).data("id");
		var RegionC = $(this)
			.parent()
			.parent()
			.find(".btndelect")
			.data("id");
		var url = "/Company/GetcompanyRegionedit?Id=" + ComeCode + "&re=" + RegionC;
		$.ajax({
			url: url,
			data: {
				data: "data"
			},
			type: "GET",
			success: function (data) {
				var DT = data;
				for (i = 0; i < DT.length; i++) {
					var rec = DT[i];
					$("#CompCode").val(rec.CompCode);
					$("#RegionCode").val(rec.RegionCode);
					$("#NameTH").val(rec.NameTH);
					$("#NameEN").val(rec.NameEN);
					$("#Head1").val(rec.Head);
					updateElement();
				}
			}
		});
		return false;
	});

	$("#tablecompany").on("click", "fa-globe", function (e) {});
	$("#btnHideModal").on("click", () => {
		$("#kt_modal_4_2").modal("hide");
	});
	$("#btnHideModal").on("click", () => {
		$("#kt_modal_4_2").modal("hide");
	});
	$("#Btncm").on("click", () => {
		Updatecompany();
	});
	$("#Btnbanch").on("click", () => {
		Updatecompanybranch();
	});
	$("#Addcompany").on("click", () => {
		$("#kt_modal_4_2").modal("show");
		$("#formcm")[0].reset();
		$("#Statusform").val("Add");
		updateElement();
	});
};
var updateElement = function updateElement() {
	$('.kt-select2').select2({
		placeholder: "Select a state"
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
		success: function (msg) {
			if (msg.success) {
				$("#kt_modal_4_2").modal("hide");
				if (msg.message == "ข้อมูลซ้ำ") {
					toastr.warning(msg.message, "แจ้งเตือน");
				} else {
					toastr.success(msg.message, "แจ้งเตือน");
					dataTablecm.ajax.reload();
				}
			} else {
				$("#kt_modal_4_2").modal("hide");
				toastr.error(msg.message, "แจ้งเตือน");
			}
		},
		error: function (errormessage) {
			//do something else
			toastr.error(errormessage, "แจ้งเตือน");
		}
	});
};
///--END COMPANY SCRIPT----------------------------------------------------------------------------------

///- START BRANCH SCRIPT----------------------------------------------------------------------------------
var Updatecompanybranch = function Updatecompanybranch() {
	var valdata = $("#formcm").serialize();
	$.ajax({
		url: "/Company/SaveCompanybranch",
		type: "POST",
		dataType: "json",
		contentType: "application/x-www-form-urlencoded; charset=UTF-8", // this
		data: valdata,
		success: function (msg) {
			if (msg.success) {
				$("#kt_modal_4_2").modal("hide");
				dataTablebn.ajax.reload();
				toastr.success(msg.message, "แจ้งเตือน");
			} else {
				$("#kt_modal_4_2").modal("hide");
				toastr.error(msg.message, "แจ้งเตือน");
			}
		},
		error: function (errormessage) {
			//do something else
			toastr.error(errormessage, "แจ้งเตือน");
		}
	});

	$(function () {
		$("#ShowModal").on('click', (e) => {
			$("#SearchString").val('');
			$.ajax({
				url: "/AssetPlantSugar/SearchPlantCode",
				type: "GET",
				dataType: "html",
				success: function (response) {
					$("#Preview").html(response);
				},
			}).then(function () {
				$("#ModalSelectPlant").modal('show');
			});
		});
		$("#btSearch").on('click', (e) => {
			e.preventDefault();
			var valsearch = $("#SearchString").val();
			var datas = {
				"currentFilter": valsearch,
				"searchString": valsearch,
				"pageNumber": 0
			};
			GetPlant(datas)
		});

		function GetPlant(dataSearch) {
			$.ajax({
				url: "/AssetPlantSugar/SearchPlantCode",
				type: "GET",
				dataType: "html",
				contentType: "application/json; charset=utf-8",
				data: dataSearch,
				success: function (response) {
					$("#Preview").html(response);
				},
			}).then(function () {
				$("#ModalSelectPlant").modal('show');
			});
		};
	});


};