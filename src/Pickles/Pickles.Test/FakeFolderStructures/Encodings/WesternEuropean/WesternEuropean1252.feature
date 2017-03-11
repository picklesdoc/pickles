#language: no
Egenskap: ISO-8859-1 CP 1252 Norwegian with letters ÆØÅ
	In norwegian, we use the letters æøå and ÆØÅ.
    It should be parsable even without BOM.

@mytag
Scenario: A scenario with æøå
	Gitt at jeg skriver æ
    Når jeg trykker ø
    Så kan jeg også parse å