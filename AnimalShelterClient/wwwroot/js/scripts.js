const showModal = modalId => {
  [...document.querySelectorAll(`.modal`)].forEach(modalElement => {
    modalElement.classList[modalElement.id !== modalId ? 'add' : 'remove']('obscured');
  });
}
const hideModal = modalId => {
  document.getElementById(`${modalId}`).classList.add('obscured');
}