const showModal = modalId => {
  [...document.querySelectorAll(`.modal`)].forEach(modalElement => {
    modalElement.classList.add('obscured');
  });
  document.getElementById(`${modalId}`).classList.remove('obscured');
}
const hideModal = modalId => {
  document.getElementById(`${modalId}`).classList.add('obscured');
}

function showDynamicModal(modalType, animalId) {
  fetch(`/Home/${modalType}Modal?animalId=${animalId}`)
    .then(response => response.text())
    .then(html => {
      const modalContainer = document.getElementById('modal-container');
      modalContainer.innerHTML = html;
      showModal(modalType);
    })
    .catch(error => console.error(error));
}