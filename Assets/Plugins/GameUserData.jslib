mergeInto(LibraryManager.library, {

    saveDataJS: function() {
		$.ajax({
            type: "POST",
            cache: false,
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            url: "https://meirkids.co.il/asp/Hannukah82GameEscapeFinish.asp",
            data: "FirstName=HanukkahEscape&LastName=AAA&Phone=080808&Address=AAA&Email=AAA@AAA.AAA&PurimComment=1234&AnyComment=5679",
            success: function (value) {
			
			}
        }) 
    },

})