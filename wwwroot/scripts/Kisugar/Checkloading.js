$(document).ready(function () {
	$("#Savesubmit").on("click", () => {
		var RegId = $("#RegionCode").val();
		var SalId = $("#SaleId").val();
		var Compcode = $("#CompCode").val();
		var BranCode = $("#BranchCode").val();
		var SaleName = $("#SaleName").val();
		var BankName = $("#BankName").val();
		var BankCode = $("#BankCode").val();
		var BranchName = $("#BranchName").val();
		var TypeCode = $("#TypeCode").val();
		var Description = $("#Description").val();
		var RunningId = $("#RunningId").val();
		var RunningName = $("#RunningName").val();
		if (
			RegId == "" ||
			SalId == "" ||
			Compcode == "" ||
			BranCode == "" ||
			SaleName == "" ||
			BankName == "" ||
			BankCode == "" ||
			BranchName == "" ||
			TypeCode == "" ||
			Description == "" ||
			RunningId == "" ||
			RunningName == ""

		) {
			return;
		} else {
			$(".overlay").removeClass("hidden");
			$(".fa-refresh").removeClass("hidden");
		}
	});
});