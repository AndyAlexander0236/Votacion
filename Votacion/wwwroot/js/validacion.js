	$(document).ready(function () {
		mostrarModalVotarAqui();
		});

	function mostrarModalVotarAqui() {
		// Lógica para abrir el modal
		$('#modalVotar').modal('show');
		}

	function irAListadoVotantes() {
		window.location.href = '/Votante/ListadoVotante';
		}


	function validarDocumento() {
			// Lógica para validar el documento de identidad
			var documentoIdentidad = $('#documentoIdentidad').val();

	// Hacer una llamada AJAX al servidor para validar el documento
	$.ajax({
		url: '/Votaciones/ValidarDocumento',
	type: 'POST',
	data: {documentoIdentidad: documentoIdentidad },
	success: function (result) {
					if (result.success) {
		// La validación fue exitosa, puedes mostrar un mensaje de bienvenida
		alert('¡Bienvenido, ' + result.nombreVotante + '!');
	// Cerrar el modal después de la validación
	$('#modalVotar').modal('hide');
					} else {
		// La validación falló, puedes mostrar un mensaje de error
		alert('Documento de identidad no válido. Inténtalo de nuevo.');
					}
				},
	error: function () {
		// Manejar errores de la llamada AJAX
		alert('Error al validar el documento de identidad.');
				}

			});
		}


$(document).ready(function () {
	$('#Votaciones').DataTable({
		"columns": [
			null, // Columna 1
			null, // Columna 2
		]
	});
});
