$moduleName = 'PSRabbitMq.Consumer'
$publishDir = Join-Path $BuildRoot "src/$moduleName/bin/Debug/netstandard2.1/publish"
$moduleDll = Join-Path $publishDir "$moduleName.dll"
$modulePsd1 = Join-Path $publishDir "$moduleName.psd1"
$pidFile = Join-Path $BuildRoot '.pid'

task . ClosePSCore,BuildModule,RunPSCore

$buildModuleParams = @{
    Inputs = {Get-ChildItem "src/$moduleName/*.cs", "src/$moduleName/$moduleName.csproj", "src/$moduleName/$moduleName.psd1"}
    Outputs = $moduleDll
}

task BuildModule @buildModuleParams {
    exec { dotnet publish "src/$moduleName"}
}



task RunPSCore {
    $psCore = @{
        FilePath = "{0}" -f (Get-Command pwsh).Source
        WorkingDirectory = $BuildRoot
        ArgumentList = @(
            '-NoExit'
            '-Command'
            '"ipmo {0}; gcm -mod {1}"' -f $modulePsd1, $moduleName
        )
    }
    $psCore
    $proc = Start-Process @psCore -PassThru
    $proc.Id | Out-File $pidFile
}

task ClosePSCore {
    Get-Content $pidFile -ErrorAction SilentlyContinue | ForEach-Object {
        Stop-Process -Id $_ -ErrorAction SilentlyContinue
    }
}