﻿using Application.Commands;
using Application.Commands.Handlers;
using Application.Exceptions;
using Domain.Repositories;
using Moq;
using Xunit;

namespace ChessTests.Application.Commands
{
    public class MakeAMoveCommandTests
    {
        [Fact]
        public async Task HandleAsyncShouldThrowTheBoardDoesntExistException()
        {
            var repo = new Mock<IChessRepository>();
            repo.Setup(r => r.GetBoard(It.IsAny<Guid>())).Returns(async (Guid id) => null);
            var commandHandler = new MakeAMoveCommandHandler(repo.Object);
            var command = new MakeAMoveCommand(Guid.NewGuid(), Guid.NewGuid(),1,1,1,1);
            await Assert.ThrowsAsync<TheBoardDoesntExistException>(async()=>await commandHandler.Handle(command,CancellationToken.None));
        }
    }
}
