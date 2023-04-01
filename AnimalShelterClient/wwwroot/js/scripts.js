const showModal = modalId => {
  [...document.querySelectorAll(`.modal`)].forEach(modalElement => {
    modalElement.classList[modalElement.id !== modalId ? 'add' : 'remove']('obscured');
  });
  document.querySelector("main").classList.add('veiled')
}
const hideModal = modalId => {
  document.getElementById(`${modalId}`).classList.add('obscured');
  document.querySelector("main").classList.remove('veiled')
}