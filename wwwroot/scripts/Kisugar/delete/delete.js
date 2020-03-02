$(
	(function() {
		var url;
		var redirectUrl;
		var target;
		//Delete Action
		$(".delete").on("click", e => {
			e.preventDefault();

			target = e.target;
			var Id = $(target).data("id");
			var controller = $(target).data("controller");
			var action = $(target).data("action");
			var Id1 = $(target).data("id1");
			var Id2 = $(target).data("id2");
			var bodyMessage = $(target).data("body-message");
			redirectUrl = $(target).data("redirect-url");
			if (typeof Id1 == "undefined" && typeof Id2 == "undefined") {
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
					Id2;
			}
			swal({
				title: "แจ้งเตือน",
				text: "คุณต้องการลบรายการนี้ใช่หรือไม่",
				icon: "warning",
				buttons: ["ไม่แน่ใจ, ยกเลิก!", "แน่ใจ, ลบเดี๋ยวนี้!"],
				dangerMode: true
			}).then(willDelete => {
				if (willDelete) {
					$.ajax({
						url: url,
						type: "GET"
					}).done(function(response) {
						if (response.success) {
							swal("แจ้งเตือน", "ลบสำเร็จ", {
								icon: "success"
							});
							$(".overlay").addClass("hidden");
							$(".fa-refresh").addClass("hidden");
							if (!redirectUrl) {
								return $(target)
									.parent()
									.parent()
									.hide("slow");
							}
							window.location.href = redirectUrl;
						} else {
							$(".overlay").addClass("hidden");
							$(".fa-refresh").addClass("hidden");
							toastr.error(msg.message, "แจ้งเตือน");
						}
					});
				} else {
					$(".overlay").addClass("hidden");
					$(".fa-refresh").addClass("hidden");
				}
			});
		});
	})()
);
