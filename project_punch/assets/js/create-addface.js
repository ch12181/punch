const video = document.querySelector('.player');
function getVideo() {
	navigator.mediaDevices.getUserMedia({ video: true, audio: false })
		.then(localMediaStream => {
			video.srcObject = localMediaStream;
			video.play();
		})
		.catch(err => {
			console.error(`OH NO!!!`, err);
		});
}

getVideo();
const canvas = document.querySelector('.photoCanvas');
const strip = document.querySelector('.strip');
var data = new Array();

var i = 0;
function takePhoto() {
	if (i >= 6) {
		alert("最多拍6張");
		return;
	}
	canvas.width = video.videoWidth;
	canvas.height = video.videoHeight;
	var ctx = document.getElementById("photoCanvas").getContext("2d").drawImage(video, 0, 0, canvas.width, canvas.height);
	
	// 將畫面輸出為 DataURL 格式的資料
	data[i] = canvas.toDataURL('image/png');
	// 製作連結
	const link = document.createElement('a');
	link.href = data[i];
	// 將連結以圖片表示
	link.innerHTML = `<img src=${data[i]} alt="selfy" width = "160" id = "pic">`;
	strip.insertBefore(link, strip.firstChild);
	setTimeout(console.log(i), 2000);
	i++;
}

const subscriptionKey = "6b310346475f44b9a97a7bca88a1f9d4";
const pg = "test1";

function createPerson() {
	var persons = "/persons"
	var empName = $("#id").val();
	var urlBase = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/";
	$.ajax({
		url: urlBase + pg + persons,
		// Request headers.
		beforeSend: function (xhrObj) {
			xhrObj.setRequestHeader("Content-Type", "application/json");
			xhrObj.setRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
		},
		async: false,
		type: "POST",
		// Request body.
		data: JSON.stringify({ "name": empName }),
	}).done(function (data) {
		$("#personID").val(data.personId);
	}).fail(function (jqXHR, textStatus, errorThrown) {
		// Display error message.
		var errorString = (errorThrown === "") ?
			"Error. " : errorThrown + " (" + jqXHR.status + "): ";
		errorString += (jqXHR.responseText === "") ?
			"" : (jQuery.parseJSON(jqXHR.responseText).message) ?
				jQuery.parseJSON(jqXHR.responseText).message :
				jQuery.parseJSON(jqXHR.responseText).error.message;
		alert(errorString);
		});
}

function addface(j) {
	
	createPerson();
	var personId = $("#personID").val();
	console.log(personId);
	var urlBase = "https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/";;
	
	fetch(data[j])
		.then(res => res.blob())
		.then(blobData => {
			$.post({
				url: urlBase + pg + "/" + "persons" + "/" + personId + "/persistedFaces",
				contentType: "application/octet-stream",
				headers: {
					'Ocp-Apim-Subscription-Key': subscriptionKey
				},
				processData: false,
				async: false,
				data: blobData
			}).done(function (data) {
				text = JSON.stringify(data);
			}).fail(function (jqXHR, textStatus, errorThrown) {
				// Display error message.
				var errorString = (errorThrown === "") ?
					"Error. " : errorThrown + " (" + jqXHR.status + "): ";
				errorString += (jqXHR.responseText === "") ?
					"" : (jQuery.parseJSON(jqXHR.responseText).message) ?
						jQuery.parseJSON(jqXHR.responseText).message :
						jQuery.parseJSON(jqXHR.responseText).error.message;
				alert(errorString);
			});
		})

}

function train() {
	
	var urlBase =
		"https://eastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/";
	$.ajax({
		url: urlBase + pg + "/train",
		// Request headers.
		beforeSend: function (xhrObj) {
			xhrObj.setRequestHeader("Content-Type", "application/json");
			xhrObj.setRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
		},
		type: "POST",
		// Request body.
		data: {},
	}).done(function (data) {
		alert("訓練完成");
	}).fail(function (jqXHR, textStatus, errorThrown) {
		// Display error message.
		var errorString = (errorThrown === "") ?
			"Error. " : errorThrown + " (" + jqXHR.status + "): ";
		errorString += (jqXHR.responseText === "") ?
			"" : (jQuery.parseJSON(jqXHR.responseText).message) ?
				jQuery.parseJSON(jqXHR.responseText).message :
				jQuery.parseJSON(jqXHR.responseText).error.message;
		alert(errorString);
	});
}

function faceapi() {

	var j;
	for (j = 0; j < 6; j++) {
		addface(j);
	}

	train();
}
