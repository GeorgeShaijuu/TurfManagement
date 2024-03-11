Imports MySql.Data.MySqlClient

Public Class Form3
    Private connectionString As String = "Server=localhost;Database=TurfManagement;User ID=root;Password=admin;"
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadTurfData()

        ' Add time options to Guna2ComboBox1
        For i As Integer = 1 To 4 ' You can adjust the range based on your requirement
            Guna2ComboBox1.Items.Add(i.ToString() + " hour(s)")
        Next
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        LoadTurfData()
    End Sub

    Private Sub LoadTurfData()
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Dim query As String = "SELECT * FROM Turfs"
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

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        MakeTurfBooking()
    End Sub

    Private Sub MakeTurfBooking()
        ' Assuming Guna2DataGridView1 is the name of your DataGridView
        If Guna2DataGridView1.SelectedRows.Count > 0 Then
            ' Get the selected turf details
            Dim selectedTurfId As Integer = Convert.ToInt32(Guna2DataGridView1.SelectedRows(0).Cells("turfId").Value)
            Dim costPerHour As Decimal = Convert.ToDecimal(Guna2DataGridView1.SelectedRows(0).Cells("Costperhour").Value)

            ' Get the selected time duration from Guna2ComboBox1
            Dim selectedTime As Integer
            Dim selectedTimeText As String = Guna2ComboBox1.Text.Trim()

            ' Extract the numeric part of the selected time text
            If selectedTimeText.EndsWith(" hour(s)") AndAlso Integer.TryParse(selectedTimeText.Substring(0, selectedTimeText.IndexOf(" ")), selectedTime) Then
                ' Calculate total price
                Dim totalPrice As Decimal = selectedTime * costPerHour

                ' Commit changes to TurfBooking
                CommitTurfBooking(selectedTurfId, totalPrice)

                MessageBox.Show("Booking successful!")
            Else
                MessageBox.Show("Invalid time duration selected.")
            End If
        Else
            MessageBox.Show("Please select a turf before making a booking.")
        End If
    End Sub


    Private Sub CommitTurfBooking(turfId As Integer, totalPrice As Decimal)
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Dim query As String = "INSERT INTO TurfBooking (TurfId, TotalPrice) VALUES (@turfId, @totalPrice)"
                Using cmd As New MySqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@turfId", turfId)
                    cmd.Parameters.AddWithValue("@totalPrice", totalPrice)

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class
