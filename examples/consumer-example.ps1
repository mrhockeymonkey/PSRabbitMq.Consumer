<#
    .SYNOPSIS
    This example assumes you have a local instance of rabbitmq and a queue called "test"

    docker run -it -p 5672:5672 -p 15672:15672 rabbitmq:management
#>

$VerbosePreference = 'Continue'

# create a new connection to rabbitmq
$connection = New-RabbitMqConnection -HostName localhost 

# create a new channel and configure QoS to limit the consumer to 5 unacked messages
$channel = New-RabbitMqChannel -Connection $connection | Set-RabbitMqQos -PrefetchCount 5 -Passthru

# start consuming messages from specified queue. Consumer tag is a helpful identifying when running multiple consumers
$consumer = $channel | Start-RabbitMqConsumer -QueueName 'test' -Tag $env:COMPUTERNAME

# wait for message to come in and handle each via the pipeline
$consumer | Wait-RabbitMqDelivery | ForEach-Object {
    # convert the message delivery body from binary to text
    $message = $_ | ConvertFrom-RabbitMqDelivery
    "Received Message: $message" | Write-Output
    
    # simulate some processing is taking place
    Start-Sleep 5

    if ($message -eq "should fail") {
        # you can detect potential issues by checking the redilivered flag
        if ($_.Redelivered) {
            # send a NACK without requeuing the message
            $_ | Confirm-RabbitMqDelivery -Channel $channel -Nack
        }
        else {
            # send a NACK but requeue the message so that another consumer will attempt to process
            $_ | Confirm-RabbitMqDelivery -Channel $channel -Nack -Requeue
        }
    } else {
        # send an ACK
        $_ | Confirm-RabbitMqDelivery -Channel $channel -Ack
    }
}
