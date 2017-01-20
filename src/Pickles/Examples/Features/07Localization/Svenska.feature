# language: sv
# you could also use app.config -> <specflow><language feature="sv" /></specflow> 
# to get swedish (for example) through all your features
Egenskap: Svenska - Summering
	För att slippa att göra dumma fel  
	Som räknare  
	Vill jag kunna lägga summera
 
  Scenario: Summera 5 och 7 ska vara 12
    Givet att jag har knappat in 5
    Och att jag har knappat in 7
    När jag summerar
    Så ska resultatet vara 12
  