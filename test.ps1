
$VerbosePreference = 'Continue'
$connection = New-RabbitMqConnection -HostName localhost 
$channel = New-RabbitMqChannel -Connection $connection 
Wait-RabbitMqMessage -Channel $channel -QueueName 'test' | ForEach-Object {
    $_ | ConvertFrom-RabbitMqMessage
    sleep 5
    $_ | Send-RabbitMqMessageAck -Channel $channel
}