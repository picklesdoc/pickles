FOR %%A IN (Html Dhtml Word Excel JSON) DO (
.\deploy\pickles-%1\exe\Pickles.exe -f=.\src\Pickles\Examples\ -o=.\Output\%%A\ --sn=Pickles --sv=%1 --df=%%A
if errorlevel 1 exit /b 1
)

exit /b 0
