@ECHO OFF

cd docs
rmdir Output /s /q
echo D | xcopy ..\Output Output /e
powershell -Command "(gc index_template.html) -replace 'VERSION_PLACEHOLDER', '%1' | Out-File -encoding ASCII index.html"

git add -A
git commit -m "Version %1"
cd ..