"use strict";
var KTDatatablesDataSourceAjaxServer = function () {

	var initTable1 = function () {
		var table = $('#kt_table_1');

		// begin first table
		table.DataTable({
			responsive: true,
			searchDelay: 500,
			processing: true,
			serverSide: true,
			ajax: '/Access/GetAccess',
			columns: [{
					data: 'UserId'
				},
				{
					data: 'UserName'
				},
				{
					data: 'Rname'
				},

			],
			columnDefs: [{
				targets: -1,
				title: 'Actions',
				orderable: false,
				data: 'UserName',
				render: function (data, type) {
					return "<a class='btn btn-danger btn-sm bt btcedit' data-id=" + data + "><i class='fa fa-pencil'></i> Edit</a>";
				},
			}, ],
		});
	};

	return {

		//main function to initiate the module
		init: function () {
			initTable1();
		}
	};
}();

jQuery(document).ready(function () {
	KTDatatablesDataSourceAjaxServer.init();
});