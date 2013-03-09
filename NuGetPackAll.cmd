pushd src\Pickles\Pickles.PowerShell && ..\.nuget\nuget.exe pack Pickles.nuspec -Version %1
popd
pushd src\Pickles\Pickles.MSBuild && ..\.nuget\nuget.exe pack Pickles.MSBuild.nuspec -Version %1
popd
