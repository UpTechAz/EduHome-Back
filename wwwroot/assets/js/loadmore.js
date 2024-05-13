$(document).ready(() => {
    let skipRow = 1;
    $('#loadMore').click(() => {
        /* console.log('work')*/
        $.ajax({
            method: "GET",
            url: "/teacher/loadMore",
            data: {
                skipRow: skipRow
            },
            success: (result) => {
                $('#teachers').append(result)
                skipRow++;
            }
        })
    })
})
$(document).ready(() => {
    let skipRow = 1;
    $('#loadmorecourse').click(() => {
        $.ajax({
            method: "GET",
            url: "/course/loadMore/",
            data: {
                skipRow: skipRow
            },
            success: (result) => {
                $('#courses').append(result)
                skipRow++;
            }
        })
    })
})
$(document).ready(() => {
    let skipRow = 1;
    $('#loadmorevent').click(() => {
        $.ajax({
            method: "GET",
            url: "/event/loadMore",
            data: {
                skipRow: skipRow
            },
            success: (result) => {
                $('#events').append(result)
                skipRow++;
            }
        })
    })
})
$(document).ready(() => {
    let skipRow = 1;
    $('#loadmoreblog').click(() => {
        $.ajax({
            method: "GET",
            url: "/blog/loadMore",
            data: {
                skipRow: skipRow
            },
            success: (result) => {
                $('#blogs').append(result)
                skipRow++;
            }
        })
    })
})

    
var dataEnded = true;
function checkDataEnded() {
    if (dataEnded) {
        $('#loadMoreContainer').hide();
    } else {
        $('#loadMoreContainer').show();
    }
}

function loadDataFromAPI() {
    dataEnded = false;
    checkDataEnded();
}
$(document).ready(function () {
    checkDataEnded();
});


