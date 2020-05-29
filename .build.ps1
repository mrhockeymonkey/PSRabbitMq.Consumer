$moduleName = 'PSRabbitMq.Consumer'
$moduleDir = Join-Path $BuildRoot "src/$moduleName"
$moduleTests = Join-Path $BuildRoot "src/$moduleName.Tests"
$publishDir = Join-Path $moduleDir "/bin/Debug/netstandard2.1/publish"
$moduleDll = Join-Path $publishDir "$moduleName.dll"
$modulePsd1 = Join-Path $publishDir "$moduleName.psd1"
$pidFile = Join-Path $BuildRoot '.pid'
$testOutputDir = Join-Path $BuildRoot ".nunit"

task . BuildModule
#task . ClosePSCore,BuildModule,RunPSCore

$buildModuleParams = @{
    Inputs = {Get-ChildItem "$moduleDir/*.cs", "$moduleDir/$moduleName.csproj", "$moduleDir/$moduleName.psd1"}
    Outputs = $moduleDll
}

task BuildModule @buildModuleParams {
    exec { dotnet publish "$moduleDir"}
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

task UnitTest {
    exec { dotnet test $moduleTests --test-adapter-path:. --logger:nunit --logger:appveyor }
}