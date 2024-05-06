const productImage = document.getElementById('productImage');
const addToCartText = document.getElementById('addToCart');


productImage.addEventListener('mouseout', function() {
    addToCartText.innerText = 'Add to Cart';
});


  var urunlerDiv = document.getElementById('urunler');
  var sonrakiSayfaBtn = document.getElementById('sonrakiSayfaBtn');
  var sayfaNumarasi = 1;
  var urunler = document.querySelectorAll('.product');
  var urunlerAdet = urunler.length;
  var urunlerPerSayfa = 9;

  function sayfaGoster(sayfa) {
    var baslangicIndex = (sayfa - 1) * urunlerPerSayfa;
    var bitisIndex = baslangicIndex + urunlerPerSayfa;

    for (var i = 0; i < urunlerAdet; i++) {
      if (i >= baslangicIndex && i < bitisIndex) {
        urunler[i].style.display = 'block';
      } else {
        urunler[i].style.display = 'none';
      }
    }
  }

  function nextPage() {
    sayfaNumarasi++;
    if ((sayfaNumarasi - 1) * urunlerPerSayfa < urunlerAdet) {
      sayfaGoster(sayfaNumarasi);
    } else {
      // Eğer tüm ürünler gösterildiyse, isteğe bağlı olarak başa dönebilir veya bir mesaj gösterebilirsiniz.
      sayfaNumarasi = 1;
      sayfaGoster(sayfaNumarasi);
    }
  }

  // İlk sayfayı göster
  sayfaGoster(sayfaNumarasi);

  // Düğmeye tıklandığında sonraki sayfayı göster
  sonrakiSayfaBtn.addEventListener('click', nextPage);
// Boş bir alışveriş sepeti oluştur
var shoppingBasket = [];

// Ürün eklemek için bir fonksiyon
function addToBasket(item) {
    shoppingBasket.push(item);
    renderBasket();
}

