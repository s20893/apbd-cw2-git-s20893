# University Equipment Rental System

## Opis projektu
Aplikacja konsolowa w C# służąca do obsługi uczelnianej wypożyczalni sprzętu.  
System umożliwia dodawanie sprzętu i użytkowników, wypożyczanie, zwroty oraz generowanie raportów.

Projekt został przygotowany z naciskiem na czytelny podział odpowiedzialności oraz zastosowanie podstawowych zasad projektowych (kohezja, coupling, elementy SOLID).

## Funkcjonalności

- Dodawanie użytkowników (Student, Employee)
- Dodawanie sprzętu (Laptop, Projector, Camera)
- Wypożyczanie sprzętu
- Zwrot sprzętu (z karą za opóźnienie)
- Ograniczenia liczby wypożyczeń:
    - Student: max 2
    - Employee: max 5
- Blokada wypożyczenia niedostępnego sprzętu
- Wyświetlanie:
    - wszystkich sprzętów
    - dostępnego sprzętu
    - aktywnych wypożyczeń użytkownika
    - przeterminowanych wypożyczeń
- Generowanie raportu końcowego

## Tryby działania

Program posiada dwa tryby:

1. Demo (automatyczny)  
   Pokazuje pełny scenariusz:
- dodanie danych
- poprawne wypożyczenie
- błędy (limit, dostępność)
- zwroty (z karą i bez)
- raport końcowy

2. Tryb interaktywny  
   Menu pozwalające na:
- dodawanie użytkowników i sprzętu
- przegląd danych (users, students, employees)
- wykonywanie operacji ręcznie

## Struktura projektu

Project/
├── Equipments/   -> modele sprzętu  
├── Users/        -> modele użytkowników  
├── Loans/        -> wypożyczenia  
├── Services/     -> logika biznesowa + menu  
├── Rules/        -> reguły (np. kara)  
└── Program.cs    -> uruchomienie aplikacji

## Decyzje projektowe

Podział odpowiedzialności (kohezja)  
Każda klasa ma jedną odpowiedzialność:
- Equipment, User, Loan → dane
- LoanService → logika wypożyczeń
- EquipmentService → zarządzanie sprzętem
- ReportService → raporty
- MenuService → interakcja z użytkownikiem

Coupling (sprzężenie)  
Zastosowano interfejs IPenaltyCalculator.  
Dzięki temu logika naliczania kar może być łatwo zmieniona bez modyfikacji LoanService.

Elementy SOLID
- SRP (Single Responsibility) → każda klasa odpowiada za jedną rzecz
- OCP (Open/Closed) → możliwość dodania nowej strategii kary bez zmiany kodu
- DIP (Dependency Inversion) → LoanService korzysta z interfejsu zamiast konkretnej implementacji

## Uruchomienie

dotnet run

## Wymagania

- .NET SDK (np. .NET 8)

## Autor

Projekt wykonany w ramach ćwiczenia z programowania obiektowego w C#.

## PS
Zrobilem blad w branchu i zle go nazwalem.... zamiast jakiegos feature-menu wyszlo feature equipment bo poczatkowo chcialem zrobic dla innego featura