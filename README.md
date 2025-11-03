# Real Robot – Week 8

Projektet er blevet udvidet, så **robot-IP og “dry run”-tilstand styres fra GUI’en**.  
Den samme app fra uge 7 kan nu bruges med:

- URSim (Docker, via fx `127.0.0.1`).
- En fysisk UR-robot (ved at skifte IP-adresse i GUI’en).
- Ren *dry run*, hvor der kun logges, hvad der ville blive sendt.

Videoen **Presentation week 8** viser først kørsel mod URSim (Dry run slået fra, robotten bevæger sig),  
og bagefter en dry-run, hvor der kun logges, hvad der ville blive sendt til en fysisk robot.

Flowdiagrammet i `Flowdiagram.png` opsummerer GUI → OrderBook → Robot-flowet.
