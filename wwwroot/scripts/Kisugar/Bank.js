$(document).ready(function() {
	App();
});
var App = function() {
	dataTablebnk = $("#tablebank").DataTable({
		bDestroy: true, //ทำลายอันเก่าทิ้งแล้วรอคำสั่ง reload
		paging: true, //showpaging
		lengthChange: false,
		searching: false,
		ordering: true,
		info: true,
		autoWidth: false,
		/* 	columns: [
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
					return "<a class='fa fa-hand-o-up btn'></a>";
				}
			},
			{
				width: "20px",
				orderable: false,
				className: "text-center",
				render: function(data, type) {
					return "<a class='fa fa-globe btn'></a>";
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
		], */

		//'select': {
		//    'style': 'multi'
		//},
		order: [[1, "asc"]],
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
};
