#language: no
Egenskap: Charset UTF8 Codepage 65001 inkl. BOM - Norwegian with letters ÆØÅ
	In norwegian, we use the letters æøå and ÆØÅ.
    It should be parsable even without BOM.

@mytag
Scenario: A scenario with æøå
	Gitt at jeg skriver æ
    Når jeg trykker ø
    Så kan jeg også parse å