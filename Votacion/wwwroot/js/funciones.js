//Funciones para fecha:

//Función que obtiene la fecha actual
function fechaActual() {
	var fecha = new Date();
	var dd = fecha.getDate();
	var mm = fecha.getMonth() + 1; //Enero es 0!
	var yyyy = fecha.getFullYear();

	if (dd < 10) {
		dd = '0' + dd;
	}

	if (mm < 10) {
		mm = '0' + mm;
	}

	fecha = mm + '/' + dd + '/' + yyyy;
	return fecha
}

//Funcion que calcula la edad dada la fecha de nacimiento
function calculaEdad() {
	var nacimiento = document.getElementById("fechanacimiento");

	nacimiento = new Date(nacimiento.value);
	actualidad = new Date(fechaActual());

	var edad = (actualidad.getFullYear() - nacimiento.getFullYear());

	if (actualidad.getMonth() < nacimiento.getMonth() || actualidad.getMonth() == nacimiento.getMonth() && actualidad.getDate() < nacimiento.getDate()) {
		edad--;
	}
	localStorage.setItem("edad", edad);
	if (edad >= 16) {
		document.getElementById("aviso1").innerHTML = "Genial, tienes " + edad + " años, puedes participar en votaciones!"
		document.getElementById("aviso1").style.backgroundColor = "Green"
	}
	else {
		document.getElementById("aviso1").innerHTML = "Lo sentimos, debes ser mayor";
		document.getElementById("aviso1").style.backgroundColor = "Yellow"
	}


	return edad;
}

//Validar si la fecha introducida es real

function existeFecha() {
	let fecha = document.getElementById('fechanacimiento').value;
	nacimiento = new Date(fecha);


	if (nacimiento.getMonth() > 12 || nacimiento.getMonth() <= 0 || nacimiento.getDate <= 0 || nacimiento > 31 || nacimiento.getFullYear <= 0) {
		return false
	}
	return true;
}

function habilita() {
	document.getElementById("botonC").disabled = false;
}

let nombreContacto, apellidoContacto, emailContacto, descripcionContacto;


