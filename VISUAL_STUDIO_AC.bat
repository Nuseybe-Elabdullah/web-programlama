@echo off
echo ====================================
echo VISUAL STUDIO'DA PROJE ACILIYOR...
echo ====================================
echo.

set SOLUTION_PATH="c:\Users\nusayba\Downloads\wetransfer_gym_2025-12-15_1709 (2)\gymv1\gym\GymManagementSystem.sln"

if exist %SOLUTION_PATH% (
    echo Solution dosyasi bulundu!
    echo Visual Studio aciliyor...
    start "" %SOLUTION_PATH%
    echo.
    echo Visual Studio'da proje acildi!
    echo.
    echo SONRAKI ADIMLAR:
    echo 1. Visual Studio yuklenene kadar bekleyin
    echo 2. Yesil Play butonuna tiklayin veya F5'e basin
    echo 3. Uygulama tarayicida acilacak
    echo.
) else (
    echo HATA: Solution dosyasi bulunamadi!
    echo Beklenen konum: %SOLUTION_PATH%
    echo.
)

pause
