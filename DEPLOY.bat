@echo off
echo ====================================
echo GYM MANAGEMENT SYSTEM - DEPLOYMENT
echo ====================================
echo.

REM Git kontrolu
git --version >nul 2>&1
if errorlevel 1 (
    echo HATA: Git yuklu degil!
    echo Git'i indirin: https://git-scm.com/download/win
    pause
    exit /b 1
)

echo [1/4] Git repository olusturuluyor...
git init
git add .
git commit -m "Initial commit - Gym Management System"

echo.
echo [2/4] GitHub'a yuklemek icin:
echo 1. GitHub.com'da yeni bir repository olusturun
echo 2. Repository URL'sini kopyalayin
echo.
set /p REPO_URL="GitHub Repository URL'sini girin: "

echo.
echo [3/4] GitHub'a yukleniyor...
git branch -M main
git remote add origin %REPO_URL%
git push -u origin main

echo.
echo [4/4] TAMAMLANDI!
echo.
echo SONRAKI ADIMLAR:
echo 1. Railway.app'e gidin ve GitHub ile giris yapin
echo 2. "New Project" tiklayin
echo 3. GitHub repository'nizi secin
echo 4. PostgreSQL database ekleyin
echo 5. Deploy butonuna basin!
echo.
pause
