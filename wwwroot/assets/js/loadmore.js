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
            url: "/course/loadMore",
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


