Assert ($PSVersionTable.PSVersion.Major -ge 7) "Powershell 7.0 or greater required!"

$moduleName = 'PSRabbitMq.Consumer'
$pidFile = Join-Path $BuildRoot '.pid'
$moduleDir = Join-Path $BuildRoot "src/$moduleName"
$moduleTests = Join-Path $BuildRoot "src/$moduleName.Tests"
$modulePsd1 = Join-Path $moduleDir "$moduleName.psd1"
$moduleVersion = [System.Version]::New((Get-Content $modulePsd1 -Raw | Invoke-Expression).ModuleVersion)
$publishRoot = Join-Path $BuildRoot "publish"
$publishDir = Join-Path $publishRoot "$moduleName/$moduleVersion"
$publishDll = Join-Path $publishDir "$moduleName.dll"
$publishPsd1 = Join-Path $publishDir "$moduleName.psd1"

$buildModuleParams = @{
    Inputs = {(Get-ChildItem "$moduleDir/*.cs" -Recurse) + (Get-ChildItem "$moduleDir/$moduleName.csproj")}
    Outputs = $publishDll
}

task . ClosePSCore,BuildModule,RunPSCore

task BuildModule @buildModuleParams {
    exec { dotnet publish "$moduleDir" --output $publishDir}
    Copy-Item -Path $modulePsd1 -Destination $publishPsd1
}

task PublishModule BuildModule,{
    Assert ($env:NUGET_API_KEY) "Missing 'NUGET_API_KEY' environment variabe!"
    [System.Environment]::SetEnvironmentVariable("PSModulePath", $publishRoot)
    Publish-Module -Name $moduleName -NuGetApiKey $env:NUGET_API_KEY -RequiredVersion $moduleVersion
}

task RunPSCore {
    $psCore = @{
        FilePath = "{0}" -f (Get-Command pwsh).Source
        WorkingDirectory = $BuildRoot
        ArgumentList = @(
            '-NoExit'
            '-Command'
            '"ipmo {0}; gcm -mod {1}"' -f $publishPsd1, $moduleName
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

task UnitTest ClosePSCore,{
    exec { dotnet test $moduleTests --test-adapter-path:. --logger:nunit --logger:appveyor }
}
