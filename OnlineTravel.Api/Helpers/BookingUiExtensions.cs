namespace OnlineTravel.Api.Helpers;

public static class BookingUiExtensions
{
	public static string GetStatusBadgeClass(this string status) => status switch
	{
		"Confirmed" => "bg-success-subtle text-success",
		"Cancelled" => "bg-danger-subtle text-danger",
		"Expired" => "bg-secondary-subtle text-secondary",
		"PendingPayment" => "bg-warning-subtle text-warning",
		_ => "bg-light text-dark"
	};

	public static string GetStatusAlertClass(this string status) => status switch
	{
		"Confirmed" => "alert-success",
		"Cancelled" => "alert-danger",
		"Expired" => "alert-secondary",
		"PendingPayment" => "alert-warning",
		_ => "alert-light"
	};

	public static string GetStatusIconClass(this string status) => status switch
	{
		"Confirmed" => "fa-check-circle",
		"Cancelled" => "fa-ban",
		"Expired" => "fa-history",
		"PendingPayment" => "fa-clock",
		_ => "fa-circle"
	};

	public static string GetPaymentBadgeClass(this string paymentStatus) => paymentStatus switch
	{
		"Paid" => "text-success",
		"Expired" => "text-secondary",
		"Due" => "text-warning",
		_ => "text-muted"
	};

	public static string GetPaymentLabelClass(this string paymentStatus) => paymentStatus switch
	{
		"Paid" => "bg-success-subtle text-success",
		"Expired" => "bg-secondary-subtle text-secondary",
		"Due" => "bg-warning-subtle text-warning",
		_ => "bg-light text-muted"
	};

	public static string GetPaymentIconClass(this string paymentStatus) => paymentStatus switch
	{
		"Paid" => "fa-check-circle",
		"Expired" => "fa-times-circle",
		_ => "fa-hourglass-half"
	};

	public static string GetTypeIconClass(this string type) => type switch
	{
		"Flight" => "fa-plane",
		"Hotel" => "fa-hotel",
		"Car" => "fa-car",
		"Tour" => "fa-globe",
		_ => "fa-question"
	};

	public static string GetTypeColorClass(this string type) => type switch
	{
		"Flight" => "bg-info-subtle text-info",
		"Hotel" => "bg-warning-subtle text-warning",
		"Car" => "bg-primary-subtle text-primary",
		"Tour" => "bg-success-subtle text-success",
		_ => "bg-light text-dark"
	};
}

