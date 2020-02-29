$(document).ready(function() {
	$(".select2").select2();
	$("#SaveSale").on("click", () => {
		$(".overlay").removeClass("hidden");
		$(".fa-refresh").removeClass("hidden");
	});
	$(".back-step").on("click", () => {
		alert("sadas");
		window.location.href = "/SaleAuth/Index";
	});
});
