async function navigateToRegistationScreen(page) {

  const registrationButtons = await page.$x("//a[contains(text(), 'Cadastre-se')]");
  const registrationButton = registrationButtons[registrationButtons.length - 1];
  await registrationButton.click();
  await page.waitForNavigation();
}

async function registerVolunteer(volunteer, page) {
  
  await page.click('#inputName');
  await page.keyboard.type(volunteer.name);
  await page.click('#inputEmail');
  await page.keyboard.type(volunteer.email);
  await page.click('#inputBirthdate');
  await fillDateField(page, volunteer.birthdate);
  await page.click('#inputCPF');
  await page.keyboard.type(volunteer.cpf);
  await page.select('#inputSex', volunteer.sex);
  await page.click('#inputProfession');
  await page.keyboard.type(volunteer.profession);
  await page.click('#inputAddress');
  await page.keyboard.type(volunteer.address);
  await page.click('#inputPhone');
  await page.keyboard.type(volunteer.phone);
  
  const inputPhoto = await page.$('#inputPhoto');
  const photoPath = `C:\\git\\VolunteeringSystem\\VolunteeringSystem\\wwwroot\\assets\\${volunteer.photo}`;
  await inputPhoto.uploadFile(photoPath);
  await page.waitFor(2000);
  
  const inputCriminalRecord = await page.$('#inputCriminalRecord');
  const criminalRecordPath = `C:\\git\\VolunteeringSystem\\VolunteeringSystem\\wwwroot\\assets\\${volunteer.criminalRecord}`;//path.relative(process.cwd(), __dirname + volunteer.criminalRecord);
  await inputCriminalRecord.uploadFile(criminalRecordPath);
  await page.waitFor(2000);
  
  await page.click('#inputPassword');
  await page.keyboard.type(volunteer.pwd);
  await page.click('#inputPasswordCheck');
  await page.keyboard.type(volunteer.pwd);
  
  await page.keyboard.press('Enter');
  await page.waitForNavigation();
  await page.waitFor(5000);
}

async function fillDateField(page, date) {
  await page.keyboard.press('Backspace');
  await page.keyboard.type(date.split('/')[0]);
  await page.waitFor(2);
  await page.keyboard.press('Backspace');
  await page.keyboard.type(date.split('/')[1]);
  await page.waitFor(2);
  await page.keyboard.press('Backspace');
  await page.keyboard.type(date.split('/')[2]);
}

module.exports = {
  navigateToRegistationScreen,
  registerVolunteer,
  fillDateField
}