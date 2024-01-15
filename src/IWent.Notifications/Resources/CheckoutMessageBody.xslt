<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="html" indent="yes" />

	<xsl:template match="/">
		<html>
			<head>
				<style>
					body {
						font-family: Arial, sans-serif;
						margin: 0;
						padding: 0;
						background-color: #f4f4f4;
					}

					.container {
						width: 80%;
						margin: auto;
						background: #fff;
						padding: 20px;
					}

					.header {
						background: #333;
						color: white;
						padding: 10px;
						text-align: center;
					}

					.footer {
						background: #333;
						color: white;
						text-align: center;
						padding: 10px;
						position: relative;
						bottom: 0;
						width: 100%;
					}

					.ticket-info {
						margin: 20px 0;
						border-bottom: 1px solid #ddd;
						padding-bottom: 20px;
					}

					table {
						width: 100%;
						border-collapse: collapse;
					}

					th, td {
						padding: 8px;
						text-align: left;
						border-bottom: 1px solid #ddd;
					}
				</style>
			</head>
			<body>
				<div class='container'>
					<div class='header'>
						<h2>Your Ticket Information</h2>
					</div>
					<h3>Dear <xsl:value-of select="CustomerName" /></h3>
					<p>Thank you for your purchase. Below are the details of your tickets:</p>
					<xsl:for-each select="Tickets/Ticket">
						<div class='ticket-info'>
							<table>
								<tr>
									<th>Event Name</th>
									<td>
										<xsl:value-of select="EventName" />
									</td>
								</tr>
								<tr>
									<th>When</th>
									<td>
										<xsl:value-of select="Occurs" />
									</td>
								</tr>
								<tr>
									<th>Where</th>
									<td>
										<xsl:value-of select="Venue" />
									</td>
								</tr>
								<tr>
									<th>Seat Number</th>
									<td>
										<xsl:value-of select="SeatNumber" />
									</td>
								</tr>
								<tr>
									<th>Price</th>
									<td>
										<xsl:value-of select="Price" />
									</td>
								</tr>
							</table>
						</div>
					</xsl:for-each>
					<div class='footer'>
						<p>Contact us at <xsl:value-of select="CompanyEmail" /> for any queries.</p>
					</div>
				</div>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
