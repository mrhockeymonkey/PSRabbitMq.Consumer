
$VerbosePreference = 'Continue'
$connection = New-RabbitMqConnection -HostName localhost 
$channel = New-RabbitMqChannel -Connection $connection | Set-RabbitMqQos -PrefetchCount 5 -Passthru
$consumer = $channel | Start-RabbitMqConsumer -QueueName 'test' -Tag $env:COMPUTERNAME

$consumer | Wait-RabbitMqDelivery | ForEach-Object {
    $message = $_ | ConvertFrom-RabbitMqDelivery
    "Received Message: $message" | Write-Output
    Start-Sleep 5

    if ($message -eq "fail") {
        if ($_.Redelivered) {
            $_ | Confirm-RabbitMqDelivery -Channel $channel -Nack
        }
        else {
            $_ | Confirm-RabbitMqDelivery -Channel $channel -Nack -Requeue
        }
    } else {
        $_ | Confirm-RabbitMqDelivery -Channel $channel -Ack
    }
}
