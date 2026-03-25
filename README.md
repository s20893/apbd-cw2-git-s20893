# University Equipment Rental System

## Opis projektu
Aplikacja konsolowa w C# służąca do obsługi uczelnianej wypożyczalni sprzętu.
System umożliwia dodawanie sprzętu, rejestrowanie użytkowników, wypożyczanie, zwroty oraz generowanie raportów.

## Struktura projektu

Projekt został podzielony na kilka głównych części:

- Models (Equipment, User, Loan) – przechowują dane
- Services – zawierają logikę biznesową
- Rules – odpowiadają za reguły naliczania kar
- Program – odpowiada za interakcję z użytkownikiem (scenariusz)

## Decyzje projektowe

Zdecydowałem się rozdzielić model domenowy od logiki biznesowej, aby zwiększyć czytelność i utrzymanie kodu.

Każda klasa ma jedną odpowiedzialność:
- Loan przechowuje dane wypożyczenia
- LoanService zarządza wypożyczeniami i zwrotami
- Equipment przechowuje dane sprzętu
- EquipmentService zarządza sprzętem

## Kohezja i coupling

Kohezja została zachowana poprzez przypisanie jednej odpowiedzialności każdej klasie.

Coupling został ograniczony poprzez zastosowanie interfejsu IPenaltyCalculator, co umożliwia zmianę logiki naliczania kar bez modyfikacji głównego systemu.

## Uruchomienie

```bash
dotnet run