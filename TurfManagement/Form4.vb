Imports MySql.Data.MySqlClient

Public Class Form4
    Private connectionString As String = "Server=localhost;Database=TurfManagement;User ID=root;Password=admin;"

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        LoadTurfBookingData()
        LoadPaymentMethodOptions()
    End Sub

    Private Sub LoadTurfBookingData()
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Dim query As String = "SELECT * FROM TurfBooking"
                Using adapter As New MySqlDataAdapter(query, connection)
                    Dim dataTable As New DataTable()
                    adapter.Fill(dataTable)

                    ' Assuming Guna2DataGridView1 is the name of your DataGridView
                    Guna2DataGridView1.DataSource = dataTable
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadPaymentMethodOptions()
        ' Add payment method options to Guna2ComboBox1
        Guna2ComboBox1.Items.Add("Credit Card")
        Guna2ComboBox1.Items.Add("Debit Card")
        Guna2ComboBox1.Items.Add("Cash")
        ' Add more options as needed
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' Check if a row is selected in the DataGridView
        If Guna2DataGridView1.SelectedRows.Count > 0 Then
            ' Get the selected BookingId from the DataGridView
            Dim selectedBookingId As Integer = Convert.ToInt32(Guna2DataGridView1.SelectedRows(0).Cells("BookingId").Value)

            ' Get the selected payment method from Guna2ComboBox1
            Dim selectedPaymentMethod As String = Guna2ComboBox1.SelectedItem.ToString()

            ' Get the amount from the selected row of the DataGridView
            Dim amount As Decimal = Convert.ToDecimal(Guna2DataGridView1.SelectedRows(0).Cells("TotalPrice").Value)

            ' Add payment details to the Payments table
            AddPaymentDetails(selectedBookingId, selectedPaymentMethod, amount)
        Else
            MessageBox.Show("Please select a booking before making a payment.")
        End If
    End Sub


    Private Sub AddPaymentDetails(bookingId As Integer, paymentMethod As String, amount As Decimal)
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Dim query As String = "INSERT INTO Payments (BookingId, PMethod, Amount) VALUES (@bookingId, @paymentMethod, @amount)"
                Using cmd As New MySqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@bookingId", bookingId)
                    cmd.Parameters.AddWithValue("@paymentMethod", paymentMethod)
                    cmd.Parameters.AddWithValue("@amount", amount)

                    cmd.ExecuteNonQuery()

                    MessageBox.Show("Payment added successfully!")
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class
