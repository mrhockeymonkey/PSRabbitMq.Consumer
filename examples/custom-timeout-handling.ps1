<#
    .SYNOPSIS
    This example demonstrates setting and catching a timeout so that you may do additional processing
    during idle time or to log output on process uptime/status

    .NOTES
    This example assumes you have a local instance of rabbitmq and a queue called "test"

    docker run -it -p 5672:5672 -p 15672:15672 rabbitmq:management
#>

$VerbosePreference = 'Continue'
$queueName = 'test'
$timeoutSeconds = 5

$connection = New-RabbitMqConnection -HostName localhost 
$channel = New-RabbitMqChannel -Connection $connection | Set-RabbitMqQos -PrefetchCount 5 -Passthru
$consumer = $channel | Start-RabbitMqConsumer -QueueName $queueName -Tag $env:COMPUTERNAME

# here we have a continuous loop alternating between waiting for emssages for 5 secondss and 
# writing some output for monitoring
while ($true) {
    try {
        $consumer | Wait-RabbitMqDelivery -Timeout $timeoutSeconds | ForEach-Object {
            # do something
        }
    }
    catch [System.TimeoutException] {
        # do nothing, this is expected
    }
    Write-Output "Still alive after $timeoutSeconds seconds"
}
