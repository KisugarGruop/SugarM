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
		var RunningMark = $("#RunningMark").val();
		var BreadName = $("#BreadName").val();
		var UnitName = $("#UnitName").val();
		var DocName = $("#DocName").val();
		var Position = $("#Position").val();
		var SubTypeCode = $("#SubTypeCode").val();
		var DocCode = $("#DocCode").val();
		var ContractCode = $("#ContractCode").val();
		var OverrideLevel = $("#OverrideLevel").val();
		var GradeCode = $("#GradeCode").val();
		var CodeName = $("#CodeName").val();
		var MinorTypeCode = $("#MinorTypeCode").val();
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
			RunningName == "" ||
			RunningMark == "" ||
			BreadName == "" ||
			UnitName == "" ||
			DocName == "" ||
			OverrideLevel == "" ||
			GradeCode == "" ||
			ContractCode == "" ||
			DocCode == "" ||
			CodeName == "" ||
			SubTypeCode == "" ||
			MinorTypeCode == "" ||
			Position == ""

		) {
			return;
		} else {
			$(".overlay").removeClass("hidden");
			$(".fa-refresh").removeClass("hidden");
		}
	});
});