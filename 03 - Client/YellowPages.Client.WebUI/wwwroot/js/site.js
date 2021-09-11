// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//#region base
var baseUrl = 'http://localhost:5000/api/gateway/';
//#endregion

//#region api endpoints
var person = 'person/';
var report = 'report/';
//#endregion

//#region common method endpoints
var getMethod = 'get/';
var getAllMethod = 'get-all';
var createOrUpdateMethod = 'create-or-update';
var deleteMethod = 'delete/';
//#endregion

//#region special api endpoints
var getPersonContactInfoByPersonIdMethod = 'get-person-contact-info-by-person-id/';
var personContactInfoCreateOrUpdate = 'person-contact-info-create-or-update';
var deletePersonContactInfoByIdMethod = 'delete-person-contact-info-by-id/';
var downloadReportMethod = 'download-report/';
//#endregion



//#region listing operations
function getPersonList() {
    var model = {
        Url: baseUrl + person + getAllMethod,
        RequestType: 'Get',
        Data: null
    };
    $.ajax({
        url: '/Home/Operator',
        type: 'Post',
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {
            var obj = JSON.parse(data);
            var tbody = $('#personTable > tbody');
            tbody.html("");
            if (obj.length > 0) {
                obj.map((item, index) => {
                    tbody.append('<tr><td>' + item.fullName + '</td><td>' + item.firmName + '</td><td> <button type="button" class="btn btn-primary btn-sm" title="Edit User" onclick="getPerson(this)" data-id="' + item.id + '"> <i class="fa fa-sm fa-fw fa-edit"></i> </button>  <button type="button" class="btn btn-danger btn-sm" title="Delete User" onclick="deletePerson(this)" data-id="' + item.id + '"> <i class="fa fa-sm fa-fw fa-times"></i> </button> </td></tr>');
                });
            } else {
                tbody.append('<tr> <td colspan="3"> No records found </td> </tr>');
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function getReportList() {
    var model = {
        Url: baseUrl + report + getAllMethod,
        RequestType: 'Get',
        Data: null
    };
    $.ajax({
        url: '/Home/Operator',
        type: 'Post',
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {
            var obj = JSON.parse(data);
            var tbody = $('#reportTable > tbody');
            tbody.html("");
            if (obj.length > 0) {
                obj.map((item, index) => {
                    tbody.append('<tr><td>' + item.createdDate + '</td> <td>' + returnReportStatus(item.reportStatus) + '</td><td> ' + returnReportDownloadButton(item.reportStatus, item.id) + ' </td></tr>');
                });
            } else {
                tbody.append('<tr> <td colspan="2"> No records found </td> </tr>');
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
//#endregion

//#region person operations
function openPersonModal() {
    $('#Id').val('');
    $('#Name').val('');
    $('#Surname').val('');
    $('#FirmName').val('');
    $('#contactInformation > tbody').html('');
    $('#personModal').modal('show');
}

function getPerson(e) {
    var id = $(e).attr('data-id');
    var model = {
        Url: baseUrl + person + getMethod + id,
        RequestType: 'Get',
        Data: null
    };
    $.ajax({
        url: '/Home/Operator',
        type: 'Post',
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {
            var obj = JSON.parse(data);
            $('#Id').val(obj.id);
            $('#Name').val(obj.name);
            $('#Surname').val(obj.surname);
            $('#FirmName').val(obj.firmName);

            var contactInfoTableBody = $('#contactInformation > tbody');
            contactInfoTableBody.html("");
            obj.personContactInfos.map((item, index) => {
                contactInfoTableBody.append('<tr> <td>' + returnContactTypeSelectBox(item.id, item.contactType) + '</td> <td> ' + returnContactInfoInput(item.id, item.contactInfo) + ' </td> <td> ' + returnContactInfoDeleteButton(item.id, obj.id) + ' </td> </tr>')
            });

            $('#personModal').modal('show');
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function deletePerson(e) {
    var id = $(e).attr('data-id');
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            var model = {
                Url: baseUrl + person + deleteMethod + id,
                RequestType: 'Delete',
                Data: null
            };
            $.ajax({
                url: '/Home/Operator',
                type: 'Post',
                data: JSON.stringify(model),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                success: function (data) {
                    if (data === 'True' || data === true || data === "true") {
                        makeAlert(true);
                        getPersonList();
                    } else {
                        makeAlert(false);
                    }
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }
    });
}

function saveOrUpdatePerson() {
    debugger;
    var personId = $('#Id').val();
    var contactInfoArray = [];
    var rows = $('#contactInformation > tbody > tr');
    if (rows.length > 0) {
        debugger;
        for (var i = 0; i < rows.length; i++) {
            var contactInfoId = $(rows[i].cells[0].children[0]).attr('data-id');
            var contactType = parseInt($(rows[i].cells[0].children[0]).val());
            var contactInfo = $(rows[i].cells[1].children[0]).val();
            contactInfoArray.push({ PersonId: personId, ContactType: contactType, ContactInfo: contactInfo, Id: contactInfoId });
        }
    }
    var data = {
        Id: personId,
        Name: $('#Name').val(),
        Surname: $('#Surname').val(),
        FirmName: $('#FirmName').val()
    };
    var model = {
        Url: baseUrl + person + createOrUpdateMethod,
        RequestType: 'Post',
        Data: JSON.stringify(data)
    };
    $.ajax({
        url: '/Home/Operator',
        type: 'Post',
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {
            if (data === 'True' || data === true || data === "true") {
                var total = 0;
                for (var i = 0; i < contactInfoArray.length; i++) {
                    debugger;
                    var model2 = {
                        Url: baseUrl + person + personContactInfoCreateOrUpdate,
                        RequestType: 'Post',
                        Data: JSON.stringify(contactInfoArray[i])
                    };
                    $.ajax({
                        url: '/Home/Operator',
                        type: 'Post',
                        data: JSON.stringify(model2),
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        },
                        success: function (data) {
                            if (data === 'True' || data === true || data === "true") {
                                total = total + 1; checkResult(total, contactInfoArray.length);
                            }
                        },
                        error: function (data) {
                            console.log(data);
                        }
                    });
                }
            } else {
                makeAlert(false);
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
//#endregion

//#region check result
function checkResult(i, t) {
    if (i === t) {
        makeAlert(true);
        $('#personModal').modal('hide');
        $('#Id').val("");
        $('#Name').val("");
        $('#Surname').val("");
        $('#FirmName').val("");
        getPersonList();
    }
}
//#endregion

//#region person contact info operations
function showContactInfos(e) {
    var id = $(e).attr('data-personid');
    var model = {
        Url: baseUrl + person + getPersonContactInfoByPersonIdMethod + id,
        RequestType: 'Get',
        Data: null
    };
    $.ajax({
        url: '/Home/Operator',
        type: 'Post',
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {
            var obj = JSON.parse(data);
            var tbody = $('#contactInformation > tbody');
            tbody.html("");
            if (obj.length > 0) {
                obj.map((item, index) => {
                    tbody.append('<tr> <td>' + returnContactTypeSelectBox(item.id, item.contactType) + '</td> <td> ' + returnContactInfoInput(item.id, item.contactInfo) + ' </td> <td> ' + returnContactInfoDeleteButton(item.id, id) + ' </td> </tr>')
                });
            } else {
                tbody.append('<tr> <td colspan="3" class="text-center"> No records found</td> </tr>');
            }
            $('#personInfoModal').modal('show');
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function deletePersonContactInfo(e) {
    var id = $(e).attr('data-id');
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            var model = {
                Url: baseUrl + person + deletePersonContactInfoByIdMethod + id,
                RequestType: 'Get',
                Data: null
            };
            $.ajax({
                url: '/Home/Operator',
                type: 'Post',
                data: JSON.stringify(model),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                success: function (data) {
                    if (data === 'True' || data === true || data === "true") {
                        makeAlert(true);
                        showContactInfos(e);
                    } else {
                        makeAlert(false);
                    }
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }
    });
}

function returnContactTtype(i) {
    switch (i) {
        case 1:
            return 'PhoneNumber';
        case 2:
            return 'EMail';
        case 3:
            return 'Location';
    }
}
//#endregion

//#region report operations
function makeReportRequest() {
    var model = {
        Url: baseUrl + report + createOrUpdateMethod,
        RequestType: 'Post',
        Data: null
    };
    Swal.fire({
        title: 'Are you sure?',
        text: "Does this report really necessary for you ?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes !'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Home/Operator',
                type: 'Post',
                data: JSON.stringify(model),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                success: function (data) {
                    if (data != null || data != "") {
                        makeAlert(true);
                    }
                    else {
                        makeAlert(false);
                    }
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }
    });
}

function downloadReport(e) {
    var id = $(e).attr('data-id');
    var model = {
        Url: baseUrl + report + downloadReportMethod + id,
        RequestType: 'Download',
        Data: null
    };
    $.ajax({
        url: '/Home/Operator',
        type: 'Post',
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {
            data = data.replace('"', '').replace('"', '');
            if (data === "") {
                makeAlert(false);
                return;
            }
            var byteCharacters = atob(data);
            var byteNumbers = new Array(byteCharacters.length);
            for (var i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            var newBlob = new Blob([byteArray], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            const downloadUrl = window.URL.createObjectURL(newBlob);
            var link = document.createElement('a');
            link.href = downloadUrl;
            link.download = "report.xlsx";
            link.click();
            link.remove();
            makeAlert(true);
        },
        error: function (data) {
            console.log(data);
        }
    });
}
//#endregion

//#region alert operations
function makeAlert(e) {
    if (e === true) {
        Swal.fire(
            'It\'s ok!',
            '',
            'success'
        );
    } else {
        Swal.fire(
            'Opps...',
            'Houston, we have a problem.',
            'error'
        );
    }
}
//#endregion

//#region return html elements
function returnContactTypeSelectBox(id, selectedType) {
    debugger;
    var html = '<select class="form-control" id="contactType_' + id + '" data-id="' + id + '">';
    html += '<option value="1"' + (selectedType === 1 ? ' selected ' : '') + '>Phone Number</option>';
    html += '<option value="2"' + (selectedType === 2 ? ' selected ' : '') + '>EMail Address</option>';
    html += '<option value="3"' + (selectedType === 3 ? ' selected ' : '') + '>Location</option>';
    html += '</select>';
    return html;
}

function returnContactInfoInput(id, value) {
    return '<input type="text" id="contactInfo_' + id + '" class="form-control" data-id="' + id + '" value="' + value + '" />';
}

function returnContactInfoDeleteButton(id, personId) {
    return '<button type="button" class="btn btn-danger btn-sm" onclick="deletePersonContactInfo(this)" data-id="' + id + '" data-personId="' + personId + '"> <i class="fa fa-sm fa-fw fa-times"></i> </button>';
}

function returnReportStatus(i) {
    switch (i) {
        case 0:
            return ' Pending ';
        case 1:
            return ' Processing ';
        case 2:
            return ' Done ';
    }
}

function returnReportDownloadButton(i, id) {
    if (i === 2) {
        return '<button type="button" class="btn btn-success btn-sm" title="Download Report" onclick="downloadReport(this)" data-id="' + id + '"> <i class="fa fa-sm fa-fw fa-download"> </i> </button>';
    } else {
        return '';
    }
}

function addBlankContactInformationRow() {
    var html = '<tr>'
    html += '<td> ' + returnContactTypeSelectBox('00000000-0000-0000-0000-000000000000', '') + ' </td>';
    html += '<td>' + returnContactInfoInput('00000000-0000-0000-0000-000000000000', '') + '</td>';
    html += '</tr>';
    $('#contactInformation > tbody').append(html);
}
        //#endregion
