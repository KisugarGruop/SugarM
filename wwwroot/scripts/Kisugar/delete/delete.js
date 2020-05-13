$(
	(function () {
		var url;
		var redirectUrl;
		var target;
		//Delete Action
		$(".delete").on("click", e => {
			e.preventDefault();

			target = e.target;
			var da = $(target).data("ta");
			var table = $(da).DataTable();
			var Id = $(target).data("id");
			var controller = $(target).data("controller");
			var action = $(target).data("action");
			var Id1 = $(target).data("id1");
			var Id2 = $(target).data("id2");
			var Id3 = $(target).data("id3");
			var Id4 = $(target).data("id4");
			var bodyMessage = $(target).data("body-message");
			redirectUrl = $(target).data("redirect-url");
			debugger;
			if (typeof Id1 == "undefined" && typeof Id2 == "undefined" && typeof Id3 == "undefined" && Id4 == "undefined") {
				url = "/" + controller + "/" + action + "?Id=" + Id;
			} else {
				url =
					"/" +
					controller +
					"/" +
					action +
					"?Id=" +
					Id +
					"&Del=" +
					Id1 +
					"&Del1=" +
					Id2 + "&Del2=" + Id3 + "&Del3=" + Id4;

			}
			debugger;
			swal.fire({
				title: 'แจ้งเตือน',
				text: "คุณต้องการลบรายการนี้ใช่หรือไม่!",
				type: 'warning',
				showCancelButton: true,
				confirmButtonText: 'แน่ใจ, ลบเดี๋ยวนี้!!'
			}).then(function (result) {
				if (result.value) {
					$.ajax({
						url: url,
						type: "GET"
					}).done(function (response) {
						if (response.success) {
							swal.fire(
								'แจ้งเตือน!',
								'ลบสำเร็จ.',
								'success'
							).then(function (e) {
								table.row($(target).parents('tr')).remove().draw("page");
							});
							$(".overlay").addClass("hidden");
							$(".fa-refresh").addClass("hidden");
							if (!redirectUrl) {
								return $(target)
									.parent()
									.parent()
									.hide("slow");
							}

						} else {
							$(".overlay").addClass("hidden");
							$(".fa-refresh").addClass("hidden");
							toastr.error(msg.message, "แจ้งเตือน");
						}
					});
				}
			});
		});
	})()
);